using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

public class StorageController : MonoBehaviour, MMEventListener<ReleaseScrew>
{
    [Header("Storage Boxes (2)")]
    public StorageBox box1;
    public StorageBox box2;

    [Header("Backup Holes (dự phòng khi Box đầy)")]
    public List<Transform> backupItems;

    private void OnEnable() => this.MMEventStartListening<ReleaseScrew>();
    private void OnDisable() => this.MMEventStopListening<ReleaseScrew>();

    public void OnMMEvent(ReleaseScrew e)
    {
        ScrewController screw = e.screwController;
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
                StartCoroutine(MoveToHole(screw, holeSlot));
                Debug.Log($"[StorageController] {screw.name} chuyển vào {holeSlot.name}");
            }
            else
            {
                Debug.LogWarning("[StorageController] Không còn chỗ trống trong các hole!");
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
    }
}
