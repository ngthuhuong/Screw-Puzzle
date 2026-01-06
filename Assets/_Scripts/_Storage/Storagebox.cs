using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

public class StorageBox : MonoBehaviour
{
    public ScrewColor acceptedColor;
    public List<Transform> screwSlots;
    private HashSet<Transform> reservedSlots = new HashSet<Transform>();

    
    private float approachDistance = 2f;
    private float moveSpeed = 3f;
    
    public MeshRenderer meshRenderer;
    private ScrewColorPresets colorPresets  ;
    
    private float moveUpDistance = 10f;
    private float transitionSpeed = 2f;
    public bool IsActive { get; private set; }
    private Vector3 originPos;


    void Awake() 
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>(true); 
        originPos = transform.position;
        if (GameManager.Instance != null)
        {
            colorPresets = GameManager.Instance.palletColor;
        }
        else
        {
            Debug.LogError("[StorageBox] GameManager.Instance không tồn tại khi Awake!");
        }
    }

    public void SetColor(ScrewColor color)
    {
        acceptedColor = color;
        IsActive = color != ScrewColor.Gray;

        if (meshRenderer != null && colorPresets != null)
        {
            meshRenderer.material = colorPresets.GetMaterial(color);
        }
    }
    public void SetInactive()
    {
        SetColor(ScrewColor.Gray);
    }
    public void ClearData()
    {
        foreach (Transform slot in screwSlots)
        {
            if (slot.childCount > 0)
            {
                Destroy(slot.GetChild(0).gameObject);
            }
        }
    }
    
    public bool CompareColor(ScrewColor color)
    {
        if (!IsActive) return false;
        return color == acceptedColor;
    }

    public bool HasSlot()
    {
        if (!IsActive) return false;
        return screwSlots.Exists(s => s.childCount == 0);
    }



    public void MoveTo(ScrewController screw)
    {
        Transform slot = GetAvailableSlot();
        if (slot == null)
        {
            Debug.LogWarning("[StorageBox] Hết slot!");
            return;
        }
        reservedSlots.Add(slot);
        StartCoroutine(MoveScrewToSlot(screw, slot));
    }


    public void MoveToFromHole(ScrewController screw, Transform targetSlot)
    {
        if (screw == null || targetSlot == null) return;
        StartCoroutine(MoveScrewToSlot(screw, targetSlot));
    }

    private Transform GetAvailableSlot()
    {
        foreach (Transform t in screwSlots)
        {
            if (!reservedSlots.Contains(t) && t.childCount == 0)
                return t;
        }
        return null;
    }

    public List<Transform> GetAllFreeSlots()
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform t in screwSlots)
            if (t.childCount == 0) list.Add(t);
        return list;
    }


    private IEnumerator MoveScrewToSlot(ScrewController screw, Transform slot)
    {
        Vector3 approachPos = slot.position + slot.up * approachDistance;
        yield return MoveSmoothly(screw.transform, approachPos);
        screw.transform.rotation = Quaternion.LookRotation(slot.forward, slot.up);
        yield return MoveSmoothly(screw.transform, slot.position);
        screw.transform.SetParent(slot, true);
        AudioManager.Instance.PlaySFX(SoundId.ScrewRelease);
        if (!HasSlot())
        {
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
    public IEnumerator MoveUp()
    {
        Vector3 start = transform.position;
        Vector3 target = start + Vector3.up * moveUpDistance;
        float t = 0f;
        AudioManager.Instance.PlaySFX(SoundId.BoxFull);
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
    public void MoveDownToOrigin()
    {
        StartCoroutine(MoveDown(originPos));
    }

    private IEnumerator MoveDown(Vector3 targetPos)
    {
        Vector3 start = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            transform.position = Vector3.Lerp(start, targetPos, t);
            yield return null;
        }
    }

    public int GetSlotCount()
    {
        if (!IsActive || screwSlots == null) 
            return 0;

        int count = 0;
        foreach (Transform slot in screwSlots)
        {
            if (slot.childCount == 0)
                count++;
        }
        return count;
    }
    
  
    public void ClearReservedSlots()
    {
        reservedSlots.Clear();
    }
}
