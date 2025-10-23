using System;
using UnityEngine;
// Đặt tên file là ScrewFace.cs
public enum ScrewFace
{
    Top,
    Bottom,
    Front,
    Back,
    Right,
    Left
    // Có thể thêm các hướng chéo nếu cần
}
// Đảm bảo struct này hiển thị trong Inspector
[Serializable]
public struct NamedRotation
{
    public ScrewFace Name; // Ví dụ: "Top", "Right", "Front"
    public Vector3 RotationEuler; // Giá trị Euler (x, y, z)
    public Vector3 PositionOffset; // Giá trị Offset (x, y, z)
}