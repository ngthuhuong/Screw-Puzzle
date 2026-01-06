using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

public class StorageController : MonoBehaviour, MMEventListener<ReleaseScrew>,MMEventListener<BoxFull>,MMEventListener<LevelSolutionReadyEvent>,MMEventListener<UseDrillTool>,MMEventListener<StartGame>,MMEventListener<UseBroomTool>
{
    [Header("Box Loader")]
    public LoadBoxes boxLoader;
    [Header("Storage Boxes (2)")]
    public StorageBox box1;
    public StorageBox box2;

    [Header("Backup Holes (dự phòng khi Box đầy)")]
    public List<Transform> backupItems;
    private int backupCount = 4; 
    
    [Header("----Tool----")]
    public DrillController drillController;

    #region ON/OF EVENT LISTENERS
    private void OnEnable()
    {
        this.MMEventStartListening<ReleaseScrew>();
        this.MMEventStartListening<BoxFull>();
        this.MMEventStartListening<LevelSolutionReadyEvent>();
        this.MMEventStartListening<UseDrillTool>();
        this.MMEventStartListening<UseBroomTool>();
        this.MMEventStartListening<StartGame>();
    } 
    private void OnDisable()
    {
        this.MMEventStopListening<ReleaseScrew>();
        this.MMEventStopListening<BoxFull>();
        this.MMEventStopListening<LevelSolutionReadyEvent>();
        this.MMEventStopListening<UseDrillTool>();
        this.MMEventStopListening<UseBroomTool>();
        this.MMEventStopListening<StartGame>();
    }
    
    #endregion
    public void OnMMEvent(ReleaseScrew e)
    {
        ScrewController screw = e.screwController;
        screw.IsInterable = false;
        ScrewColor color = screw.GetColor();

        bool stored = false;

        if (box1.HasSlot() && box1.CompareColor(color))
        {
            box1.MoveTo(screw);
            stored = true;
        }
        else if (box2.HasSlot() && box2.CompareColor(color))
        {
            box2.MoveTo(screw);
            stored = true;
        }

        if (!stored)
        {
            Transform holeSlot = GetAvailableBackupSlot();
            if (holeSlot != null)
            {
                StartCoroutine(MoveToHole(screw, holeSlot)); //kiẻm tra trong cổutine luon
            }
        }
    }
    public void OnMMEvent(LevelSolutionReadyEvent e)
    {
        if (boxLoader == null)
        {
            Debug.LogError("[StorageController] boxLoader null");
            return;
        }
        ResetStorage();
        boxLoader.SetSolution(e.solutionColors);

        InitializeBoxes();
    }
    
    private void InitializeBoxes()
    {
        if (box1 != null)
        {
            box1.SetColor(boxLoader.GetNextColor());
        }

        if (box2 != null)
        {
            box2.SetColor(boxLoader.GetNextColor());
        }
    }

#region Backup Slots

public void ResetBackupSlots()
{
    if(backupItems.Count > backupCount)
    {
        for(int i = backupItems.Count -1; i >= backupCount; i--)
        {
            Destroy(backupItems[i].gameObject);
            backupItems.RemoveAt(i);
        }
    }
}

    private Transform GetAvailableBackupSlot()
    {
        foreach (Transform t in backupItems)
            if (t.childCount == 0)
                return t;
        return null;
    }

    private IEnumerator MoveToHole(ScrewController screw, Transform hole)
    {
        if (screw == null || hole == null) yield break;

        Vector3 startPos = screw.transform.position;
        Vector3 targetPos = hole.position;
        float t = 0f;
        float moveSpeed = 3f;

        // ✅ Di chuyển mượt
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            screw.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        screw.transform.rotation = Quaternion.LookRotation(hole.forward, hole.up);
        screw.transform.SetParent(hole, true);
        screw.PlayAnim("isSpin");
        
        if (NoMoreBackupSlots())
        {
            MMEventManager.TriggerEvent(new LoseGame());
        }
    }
    private bool NoMoreBackupSlots()
    {
        foreach (Transform t in backupItems)
            if (t.childCount == 0) return false; 
        return true; 
    }
   

    public void OnMMEvent(StartGame eventType)
    {
        ResetBackupSlots();
        box1.ClearReservedSlots();
        box2.ClearReservedSlots();
    }
    
    

#endregion

#region Tools Events
public void OnMMEvent(UseDrillTool eventType)
{
    Transform lastHole = backupItems[backupItems.Count - 1];
    float offsetX = 2f;
    drillController.InitializeDrill(offsetX);
    Transform newHole = Instantiate(
        lastHole,
        lastHole.parent    // giữ chung parent
    );
    newHole.localPosition = lastHole.localPosition;
    newHole.localPosition += Vector3.right * offsetX;
    backupItems.Add(newHole);
}

public void OnMMEvent(UseBroomTool eventType)
{
    foreach (Transform backup in backupItems)
    {
        if (backup.childCount == 0) continue;
        ScrewController screw = backup.GetChild(0).GetComponent<ScrewController>();
        if (screw == null) continue;
        screw.IsInterable = false;
        screw.PlayAnim("isSweeped");
        Destroy(screw.gameObject, 1f);
            
    }
}

#endregion

#region Box Full Event

    public void OnMMEvent(BoxFull e)
    {
        e.box.ClearReservedSlots();
        StartCoroutine(HandleReplaceBox(e.box));
    }

    private IEnumerator HandleReplaceBox(StorageBox fullBox)
    {
        yield return StartCoroutine(fullBox.MoveUp());

        if (!boxLoader.HasNextBox())
        {
            fullBox.ClearData();
            fullBox.SetInactive(); // box trống, không màu
            yield break;
        }

        ScrewColor nextColor = boxLoader.GetNextColor();

        fullBox.ClearData();
        fullBox.SetColor(nextColor);
        fullBox.MoveDownToOrigin();

        yield return new WaitForSeconds(0.3f);
        TryFillFromBackup(fullBox);
    }


    
    public void TryFillFromBackup(StorageBox targetBox)
    {
        if (targetBox == null) return;

        List<Transform> matchedScrews = new List<Transform>();

        foreach (Transform backup in backupItems)
        {
            if (backup.childCount > 0)
            {
                ScrewController screw = backup.GetChild(0).GetComponent<ScrewController>();
                if (screw != null && screw.GetColor() == targetBox.acceptedColor)
                {
                    matchedScrews.Add(screw.transform);
                }
            }
        }

        if (matchedScrews.Count > 0)
            FillFromBackupInOrder(targetBox, matchedScrews);
    }

    private void FillFromBackupInOrder(StorageBox targetBox, List<Transform> screws)
    {
        List<Transform> freeSlots = targetBox.GetAllFreeSlots();
        int count = Mathf.Min(screws.Count, freeSlots.Count);

        for (int i = 0; i < count; i++)
        {
            ScrewController screw = screws[i].GetComponent<ScrewController>();
            targetBox.MoveToFromHole(screw, freeSlots[i]);
            Debug.Log($"[StorageController] (Batch) Di chuyển {screw.name} vào slot {freeSlots[i].name} của {targetBox.name}");
        }
    }

    public void ResetStorage()
    {
        ResetBox(box1);
        ResetBox(box2);

        foreach (Transform backup in backupItems)
        {
            for (int i = backup.childCount - 1; i >= 0; i--)
            {
                Destroy(backup.GetChild(i).gameObject);
            }
        }
    }

    private void ResetBox(StorageBox box)
    {
        if (box == null) return;

        box.ClearData();
        box.MoveDownToOrigin();
        box.SetInactive(); // inactive , KHÔNG disable GameObject
    }

    #endregion


}
