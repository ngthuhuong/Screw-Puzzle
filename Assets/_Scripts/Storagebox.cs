using UnityEngine;

public class StorageBox : MonoBehaviour
{
    public ScrewColor acceptedColor;

    public bool CompareColor(ScrewColor color)
    {
        return color == acceptedColor;
    }

    public void MoveTo(ScrewController screw)
    {
        screw.transform.position = transform.position;
        Debug.Log($"[StorageBox] {screw.name} đã được lưu vào box {acceptedColor}.");
    }
}