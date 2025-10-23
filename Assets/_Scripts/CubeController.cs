using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private List<ScrewController> activeScrews; // Danh sách các vít đang hoạt động
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true; 
   
        ScrewController[] childScrews = GetComponentsInChildren<ScrewController>();
        activeScrews = new List<ScrewController>(childScrews);
        
        Debug.Log(gameObject.name + " được giữ bởi: " + activeScrews.Count + " vít.");
    }

    public void ScrewRemoved(ScrewController removedScrew)
    {
        if (activeScrews.Contains(removedScrew))
        {
            activeScrews.Remove(removedScrew);
            Debug.Log(gameObject.name + ": Vít đã tháo. Còn lại " + activeScrews.Count + " vít.");
        }
        
        if (activeScrews.Count == 0)
        {
            ReleasePlank();
        }
    }

    private void ReleasePlank()
    {
        Debug.Log(gameObject.name + ": Tất cả vít đã tháo! Bắt đầu rơi tự do.");
        rb.isKinematic = false; 
        
        transform.SetParent(null);

        Destroy(gameObject, 5f); 
    }
}