using UnityEngine;
using System.Linq;

// Cho phép script chạy trong Editor để cập nhật hình ảnh ngay lập tức
[ExecuteAlways] 
public class ScrewSetup : MonoBehaviour
{
    // Cấu hình dữ liệu
    public RotationPresetsSO rotationPresets;
    public ScrewColorPresets colorPresets;
    public ScrewFace presetName;
    public ScrewColor screwColor;

    [Tooltip("Transform của đối tượng mô hình con (Chỉ chứa Mesh/Renderer).")]
    public Transform screwTransform;
    
    private Renderer screwRenderer;
    private ScrewController screwController; 

    void Awake()
    {
        // Khởi tạo các tham chiếu cần thiết khi game bắt đầu
        if(screwTransform == null)
        {
            screwTransform = this.transform;
        }
        
        // Tìm Renderer trên đối tượng hiện tại HOẶC đối tượng con
        screwRenderer = GetComponentInChildren<Renderer>(); 
        
        // Tìm ScrewController trên đối tượng Cha
        screwController = GetComponentInParent<ScrewController>();

        if (rotationPresets != null && colorPresets != null)
        {
            ApplyTransform();
        }
    }
    
    // Chạy khi thay đổi Inspector (Editor Mode)
    private void OnValidate()
    {
        if (Application.isPlaying) return; 

        // Gán lại các tham chiếu an toàn trong Editor Mode
        if (screwTransform == null)
        {
            screwTransform = this.transform;
        }
        if (screwRenderer == null)
        {
             screwRenderer = GetComponentInChildren<Renderer>();
        }
        if (screwController == null)
        {
             screwController = GetComponentInParent<ScrewController>();
        }
        
        if (rotationPresets != null && colorPresets != null)
        {
            ApplyTransform();
        }
    }

    private void ApplyTransform()
    {
        // 1. Áp dụng Rotation và Position
        if (rotationPresets != null && screwTransform != null)
        {
            NamedRotation presetData = rotationPresets.GetPresetData(presetName);
            
            // Áp dụng Rotation
            screwTransform.localRotation = Quaternion.Euler(presetData.RotationEuler);
            
            // Áp dụng Position Offset
            screwTransform.localPosition = presetData.PositionOffset;
        }
        
        // 2. Áp dụng Màu sắc
        if (colorPresets != null && screwRenderer != null)
        {
            Material targetMaterial = colorPresets.GetMaterial(screwColor);
            if (targetMaterial != null)
            {
                // Sử dụng .material để tạo Instance và thay đổi màu trong Runtime
                screwRenderer.material = targetMaterial; 
            }
        }
        
        
    }
}