using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StorageBox : MonoBehaviour
{
    public ScrewColor acceptedColor;

    public List<Transform> screwSlots;

    public float approachDistance = 2f;

    public float moveSpeed = 3f;

    public bool CompareColor(ScrewColor color)
    {
        return color == acceptedColor;
    }

    public bool HasSlot() => screwSlots.Exists(s => s.childCount == 0);



    public void MoveTo(ScrewController screw)
    {
        Transform emptySlot = GetAvailableSlot();
        if (emptySlot == null)
        {
            Debug.LogWarning("[StorageBox] Hết chỗ trống cho vít!");
            return;
        }

        StartCoroutine(MoveScrewToSlot(screw, emptySlot));
    }

    private Transform GetAvailableSlot()
    {
        foreach (Transform t in screwSlots)
            if (t.childCount == 0) return t;
        return null;
    }

    private IEnumerator MoveScrewToSlot(ScrewController screw, Transform slot)
    {
        if (screw == null) yield break;

        Vector3 approachPos = slot.position + slot.up * approachDistance;
        yield return MoveSmoothly(screw.transform, approachPos);

       

        screw.transform.rotation = Quaternion.LookRotation(slot.forward, slot.up); // ✅ canh hướng vít
        screw.transform.SetParent(slot, true); // gán vào slot sau cùng

        screw.PlayAnim("isSpin");
        yield return MoveSmoothly(screw.transform, slot.position);
    }


    private IEnumerator MoveSmoothly(Transform obj, Vector3 target)
    {
        float t = 0;
        Vector3 start = obj.position;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            obj.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
}
