using UnityEngine;

public class StorageBox : MonoBehaviour
{
    public ScrewColor acceptedColor;

    public bool CompareColor(ScrewColor color)
    {
        return color == acceptedColor;
    }

    public void MoveTo(ScrewController screw, Vector3 targetPos)
    {
        StartCoroutine(screw.MoveTo(targetPos, null));

    }

}