using System.Collections.Generic;
using UnityEngine;

// Cho phép tạo asset này qua menu Create
[CreateAssetMenu(fileName = "RotationPreset", menuName = "Game Data/Rotation Presets", order = 1)]
public class RotationPresetsSO : ScriptableObject
{
    public List<NamedRotation> Presets = new List<NamedRotation>();
    public NamedRotation GetPresetData(ScrewFace face)
    {
        foreach (var preset in Presets)
        {
            if (preset.Name == face)
            {
                return preset; // Trả về Struct chứa cả hai Vector3
            }
        }
        
        // Trường hợp không tìm thấy, trả về một struct mặc định với cảnh báo
        Debug.LogWarning($"Rotation/Position preset '{face}' not found. Returning default values.");
        return new NamedRotation 
        { 
            Name = face, 
            RotationEuler = Vector3.zero, 
            PositionOffset = Vector3.zero 
        };
    }
    
    // (Giữ lại hàm GetRotation cũ để tương thích nếu cần, hoặc xóa đi)
    public Quaternion GetRotation(ScrewFace face)
    {
        return Quaternion.Euler(GetPresetData(face).RotationEuler);
    }

    // Hàm mới để lấy riêng Position Offset
    public Vector3 GetPositionOffset(ScrewFace face)
    {
        return GetPresetData(face).PositionOffset;
    }
}