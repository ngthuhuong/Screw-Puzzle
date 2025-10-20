using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private List<ScrewController> screws; // Danh sách các vít v
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        rb.isKinematic = true; 
    }

    // Hàm được gọi từ ScrewController khi một vít bị tháo ra
    public void ScrewRemoved(ScrewController removedScrew)
    {
        // 1. Xóa vít khỏi danh sách
        if (screws.Contains(removedScrew))
        {
            screws.Remove(removedScrew);
            Debug.Log(gameObject.name + ": Vít đã tháo. Còn lại " + screws.Count + " vít.");
        }
        
        // 2. Kiểm tra nếu tất cả vít đã tháo
        if (screws.Count == 0)
        {
            ReleasePlank();
        }
    }

    private void ReleasePlank()
    {
        Debug.Log(gameObject.name + ": Tất cả vít đã tháo! Bắt đầu rơi tự do.");
        rb.isKinematic = false; 
        
         Destroy(gameObject, 5f); 
    }
}