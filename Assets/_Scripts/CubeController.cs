using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private List<ScrewController> activeScrews; // Danh sách các vít đang hoạt động
    
    private Rigidbody rb;
    private bool isReleased = false; 
    private bool isFading = false;

    public GameObject cubeModel;
    [Header("screws inside")]
    public ScrewSetup screwTop;
    public ScrewSetup screwBottom;
    public ScrewSetup screwFront;
    public ScrewSetup screwBack;
    public ScrewSetup screwRight;
    public ScrewSetup screwLeft;

    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        ScrewController[] childScrews = GetComponentsInChildren<ScrewController>();
        activeScrews = new List<ScrewController>(childScrews);

    }

    public void ScrewRemoved(ScrewController removedScrew)
    {
        if (activeScrews.Contains(removedScrew))
        {
            activeScrews.Remove(removedScrew);
        }

        if (activeScrews.Count == 0 && !isReleased)
        {
            isReleased = true;
            MMEventManager.TriggerEvent(new CubeCleared(this));
            ReleasePlank();
        }
    }

    private void ReleasePlank()
    {
        rb.isKinematic = false;
        transform.SetParent(null);
    }

    void Update()
    {
        // 🧠 Nếu đã được thả và rơi ra ngoài màn hình -> huỷ
        if (isReleased && !IsVisibleFrom(Camera.main))
        {
            Destroy(gameObject);
        }
        // Kiểm tra nhấn giữ chuột phải
        if (Input.GetMouseButton(1)) // Chuột phải
        {
            if (!isFading)
            {
                isFading = true;
                FadeChild(cubeModel, true);
            }
        }
        else if (isFading)
        {
            isFading = false;
            FadeChild(cubeModel, false);
        }
    }
    private void FadeChild(GameObject child, bool fade)
    {
        Renderer childRenderer = child.GetComponent<Renderer>();
        if (childRenderer != null)
        {
            Material childMaterial = childRenderer.material;
            Color color = childMaterial.color;

            if (fade)
            {
                color.a = 0.5f; // Set opacity to 50%
            }
            else
            {
                color.a = 1.0f; // Restore full opacity
            }

            childMaterial.color = color;
        }
    }


    private bool IsVisibleFrom(Camera cam)
    {
        if (cam == null)
            return false;

        // Kiểm tra nếu Cube nằm trong vùng nhìn thấy của camera
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);
        return (viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1);
    }
    
    //control list of screws
    public void EnableAllScrews()
    {
        screwTop.gameObject.SetActive(true);
        screwBottom.gameObject.SetActive(true);
        screwFront.gameObject.SetActive(true);
        screwBack.gameObject.SetActive(true);
        screwRight.gameObject.SetActive(true);
        screwLeft.gameObject.SetActive(true);
    }

    // Disable tất cả vít
    public void DisableAllScrews()
    {
        screwTop.gameObject.SetActive(false);
        screwBottom.gameObject.SetActive(false);
        screwFront.gameObject.SetActive(false);
        screwBack.gameObject.SetActive(false);
        screwRight.gameObject.SetActive(false);
        screwLeft.gameObject.SetActive(false);
    }

    // Enable/Disable theo hướng cụ thể
    public void SetScrewActive(ScrewFace face, bool active)
    {
        switch (face)
        {
            case ScrewFace.Top: screwTop.gameObject.SetActive(active); break;
            case ScrewFace.Bottom: screwBottom.gameObject.SetActive(active); break;
            case ScrewFace.Front: screwFront.gameObject.SetActive(active); break;
            case ScrewFace.Back: screwBack.gameObject.SetActive(active); break;
            case ScrewFace.Right: screwRight.gameObject.SetActive(active); break;
            case ScrewFace.Left: screwLeft.gameObject.SetActive(active); break;
        }
    }
    private ScrewSetup GetScrewSetup(ScrewFace face)
    {
        switch (face)
        {
            case ScrewFace.Top: return screwTop;
            case ScrewFace.Bottom: return screwBottom;
            case ScrewFace.Front: return screwFront;
            case ScrewFace.Back: return screwBack;
            case ScrewFace.Right: return screwRight;
            case ScrewFace.Left: return screwLeft;
        }
        return null;
    }


    
    public void Initialize(List<ScrewInfo> screws)
    {
        DisableAllScrews();

        foreach (var info in screws)
        {
            ScrewSetup setup = GetScrewSetup(info.direction);

            if (setup != null)
            {
                setup.ApplyColor(info.color);
                setup.gameObject.SetActive(true);
            }
        }
    }

}