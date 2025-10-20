using Unity.VisualScripting;
using UnityEngine;
[ExecuteAlways]
public class ScrewSetup : MonoBehaviour
{
    // Kéo Asset ScriptableObject đã tạo vào đây
    public RotationPresetsSO rotationPresets;
    public ScrewFace presetName;
    public Transform screwTransform;
    private ScrewController screwController;
    void Awake()
    {
        screwController = GetComponent<ScrewController>();      
        if(screwTransform == null)
        {
            screwTransform = this.transform;
        }
        if (rotationPresets != null)
        {
            ApplyTransform();
        }
    }
    private void ApplyTransform()
    {
        if (rotationPresets != null && screwTransform != null)
        {
            NamedRotation presetData = rotationPresets.GetPresetData(presetName);
            
            // 1. Áp dụng Rotation
            screwTransform.localRotation = Quaternion.Euler(presetData.RotationEuler);
            
            // 2. Áp dụng Position Offset
            screwTransform.localPosition = presetData.PositionOffset;
            
            // 3. GÁN HƯỚNG THÁO CHO SCREW CONTROLLER
            if (screwController != null)
            {
                // Gán trục cục bộ tiêu chuẩn cho ScrewController
                screwController.extractionLocalAxis = presetData.ExtractionLocalAxis;
            }
        }
    }
}
