using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

public class StorageController : MonoBehaviour, MMEventListener<ReleaseScrew>,MMEventListener<BoxFull>,MMEventListener<LevelSolutionReadyEvent>
{
    [Header("Box Loader")]
    public LoadBoxes boxLoader;
    [Header("Storage Boxes (2)")]
    public StorageBox box1;
    public StorageBox box2;

    [Header("Backup Holes (dự phòng khi Box đầy)")]
    public List<Transform> backupItems;

    
    private void OnEnable()
    {
        this.MMEventStartListening<ReleaseScrew>();
        this.MMEventStartListening<BoxFull>();
        this.MMEventStartListening<LevelSolutionReadyEvent>();

    } 
    private void OnDisable()
    {
        this.MMEventStopListening<ReleaseScrew>();
        this.MMEventStopListening<BoxFull>();
        this.MMEventStopListening<LevelSolutionReadyEvent>();
    }


    public void OnMMEvent(ReleaseScrew e)
    {
        ScrewController screw = e.screwController;
        screw.IsInterable = false;
        ScrewColor color = screw.GetColor();

        Debug.Log($"[StorageController] Nhận sự kiện tháo vít: {screw.name} ({color})");

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

    public void OnMMEvent(BoxFull e)
    {
       // StorageBox fullBox = e.box;
        StartCoroutine(HandleReplaceBox(e.box));
    }

    private IEnumerator HandleReplaceBox(StorageBox fullBox)
    {  
        Vector3 replacePos = fullBox.transform.position;
        Quaternion replaceRot = fullBox.transform.rotation;
       
        yield return StartCoroutine(fullBox.MoveUpAndDisable()); //doi hop cu di chuyen len 
 
        StorageBox newBox = boxLoader.LoadNextBox(replacePos, replaceRot);
        if (fullBox == box1) box1 = newBox;
        else if (fullBox == box2) box2 = newBox;
        yield return new WaitForSeconds(0.5f);

        // ✅ Sau đó kiểm tra backup
        TryFillFromBackup(newBox);
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

    
}
