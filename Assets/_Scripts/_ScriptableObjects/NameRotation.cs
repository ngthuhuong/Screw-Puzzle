using System;
using UnityEngine;

public enum ScrewFace
{
    Top=1,
    Bottom=2,
    Front=3,
    Back=4,
    Right=5,
    Left=6,
    None=0
}
[Serializable]
public struct NamedRotation
{
    public ScrewFace Name;
    public Vector3 RotationEuler;
    public Vector3 PositionOffset; // Giá trị Offset (x, y, z)
}