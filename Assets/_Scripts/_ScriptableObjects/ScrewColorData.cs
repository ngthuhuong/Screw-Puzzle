using UnityEngine;
using System;
using System.Collections.Generic;

// Enum đại diện cho các loại màu/chất liệu vít
public enum ScrewColor
{
    Blue=1,
    Red=2,
    Green=3,
    Orange=4,
    Purple=5,
    Gray =0
}

[Serializable]
public struct ColorSetting
{
    public ScrewColor color;
    public Material material; // Kéo vật liệu thực tế vào đây
}

[CreateAssetMenu(fileName = "ScrewColorPresets", menuName = "Game Data/Screw Color Presets")]
public class ScrewColorPresets : ScriptableObject
{
    public List<ColorSetting> colors = new List<ColorSetting>();
    
    // Hàm tiện ích để lấy Material từ Enum
    public Material GetMaterial(ScrewColor color)
    {
        foreach (var setting in colors)
        {
            if (setting.color == color)
            {
                return setting.material;
            }
        }
        Debug.LogWarning($"Material for color {color} not found.");
        return null;
    }
}