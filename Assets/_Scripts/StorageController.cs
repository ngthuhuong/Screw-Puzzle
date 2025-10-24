using UnityEngine;
using System.Collections.Generic;
using MoreMountains.Tools;

public class StorageController : MonoBehaviour, MMEventListener<ReleaseScrew>
{
    [Header("Storage Boxes (2)")]
    public StorageBox box1;
    public StorageBox box2;

    [Header("Backup Slots (4)")]
    public List<Transform> backupItems;

    private void OnEnable() => this.MMEventStartListening<ReleaseScrew>();
    private void OnDisable() => this.MMEventStopListening<ReleaseScrew>();

    public void OnMMEvent(ReleaseScrew e)
    {
        ScrewController screw = e.screwController;
        ScrewColor color = screw.GetColor();

        Debug.Log($"[StorageController] Nhận sự kiện tháo vít: {screw.name} ({color})");

        bool stored = false;

        if (box1 != null && box1.CompareColor(color))
        {
            box1.MoveTo(screw);
            stored = true;
        }
        else if (box2 != null && box2.CompareColor(color))
        {
            box2.MoveTo(screw);
            stored = true;
        }

        if (!stored)
        {
            Transform slot = GetAvailableBackupSlot();
            if (slot != null)
            {
                MoveTo(screw, slot.position);
                Debug.Log($"[StorageController] {screw.name} sai màu, chuyển vào {slot.name}");
            }
            else
            {
                Debug.LogWarning("[StorageController] Không còn chỗ trống trong backup!");
            }
        }
    }

    private Transform GetAvailableBackupSlot()
    {
        foreach (Transform t in backupItems)
            if (t.childCount == 0) return t;
        return null;
    }

    public void MoveTo(ScrewController screw, Vector3 targetPos)
    {
        screw.transform.position = targetPos;
    }
}