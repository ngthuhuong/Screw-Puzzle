using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

public class StorageBox : MonoBehaviour
{
    public ScrewColor acceptedColor;
    public List<Transform> screwSlots;
    
    public float approachDistance = 2f;
    public float moveSpeed = 3f;
    
    private MeshRenderer meshRenderer;
    public ScrewColorPresets colorPresets;
    
    public float moveUpDistance = 2.5f;   // khoảng di chuyển lên trên
    public float moveDownDistance = 2.5f; // khoảng di chuyển xuống
    public float transitionSpeed = 2f;

    void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void SetColor(ScrewColor color)
    {
        acceptedColor = color; // Cập nhật luôn màu được chấp nhận
        if (meshRenderer != null && colorPresets != null)
        {
            Material mat = colorPresets.GetMaterial(color);
            if (mat != null)
            {
                meshRenderer.material = mat;
            }
        }
        else
        {
            Debug.LogWarning($"[StorageBox] Thiếu meshRenderer hoặc colorPresets cho {name}");
        }
    }

    public void DisableBox()
    {
        // Ẩn box hoặc di chuyển đi chỗ khác
        gameObject.SetActive(false);
    }
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
        
        if (!HasSlot())
        {
            Debug.Log($"[StorageBox] {name} đã đầy, gửi sự kiện!");
            MMEventManager.TriggerEvent(new BoxFull(this));
        }
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
    public IEnumerator MoveUpAndDisable()
    {
        Vector3 start = transform.position;
        Vector3 target = start + Vector3.up * moveUpDistance;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
        DisableBox(); // Ẩn hộp sau khi ra khỏi khung nhìn
    }

    // 📦 Khi hộp mới spawn, di chuyển xuống vị trí ban đầu
    public IEnumerator MoveDownTo(Vector3 targetPos)
    {
        Vector3 start = transform.position;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            transform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }
    }
}
