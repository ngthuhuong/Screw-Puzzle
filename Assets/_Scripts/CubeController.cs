using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private List<ScrewController> activeScrews; // Danh sách các vít đang hoạt động
    private Rigidbody rb;
    private bool isReleased = false; // tránh gọi lại nhiều lần
    
    [Header("screws face")]
    public GameObject screwTop;
    public GameObject screwBottom;
    public GameObject screwFront;
    public GameObject screwBack;
    public GameObject screwRight;
    public GameObject screwLeft;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        ScrewController[] childScrews = GetComponentsInChildren<ScrewController>();
        activeScrews = new List<ScrewController>(childScrews);

        Debug.Log($"{gameObject.name} được giữ bởi: {activeScrews.Count} vít.");
    }

    public void ScrewRemoved(ScrewController removedScrew)
    {
        if (activeScrews.Contains(removedScrew))
        {
            activeScrews.Remove(removedScrew);
            Debug.Log($"{gameObject.name}: Vít đã tháo. Còn lại {activeScrews.Count} vít.");
        }

        if (activeScrews.Count == 0 && !isReleased)
        {
            isReleased = true;
            ReleasePlank();
        }
    }

    private void ReleasePlank()
    {
        Debug.Log($"{gameObject.name}: Tất cả vít đã tháo! Bắt đầu rơi tự do.");
        rb.isKinematic = false;
        transform.SetParent(null);
    }

    void Update()
    {
        // 🧠 Nếu đã được thả và rơi ra ngoài màn hình -> huỷ
        if (isReleased && !IsVisibleFrom(Camera.main))
        {
            Debug.Log($"{gameObject.name}: Rơi khỏi màn hình -> Hủy.");
            Destroy(gameObject);
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
        screwTop.SetActive(true);
        screwBottom.SetActive(true);
        screwFront.SetActive(true);
        screwBack.SetActive(true);
        screwRight.SetActive(true);
        screwLeft.SetActive(true);
    }

    // Disable tất cả vít
    public void DisableAllScrews()
    {
        screwTop.SetActive(false);
        screwBottom.SetActive(false);
        screwFront.SetActive(false);
        screwBack.SetActive(false);
        screwRight.SetActive(false);
        screwLeft.SetActive(false);
    }

    // Enable/Disable theo hướng cụ thể
    public void SetScrewActive(ScrewFace face, bool active)
    {
        switch (face)
        {
            case ScrewFace.Top: screwTop.SetActive(active); break;
            case ScrewFace.Bottom: screwBottom.SetActive(active); break;
            case ScrewFace.Front: screwFront.SetActive(active); break;
            case ScrewFace.Back: screwBack.SetActive(active); break;
            case ScrewFace.Right: screwRight.SetActive(active); break;
            case ScrewFace.Left: screwLeft.SetActive(active); break;
        }
    }
    
    public void Initialize(List<ScrewInfo> screws)
    {
        DisableAllScrews();
        // 2. Duyệt qua danh sách và Bật/Thiết lập màu
        foreach (var info in screws)
        {
            SetScrewActive(info.direction, true);
        }
    }
}