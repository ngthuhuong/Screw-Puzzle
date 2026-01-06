using System.Collections.Generic;
using UnityEngine;

public class LoadBoxes : MonoBehaviour
{
    public List<ScrewColor> boxColors;
    private int currentColorIndex;

    public void SetSolution(List<ScrewColor> solution)
    {
        boxColors = solution;
        currentColorIndex = 0;
        Debug.Log("[LoadBoxes] Solution set with " + boxColors.Count + " colors.");
    }

    public bool HasNextBox()
    {
        return boxColors != null && currentColorIndex < boxColors.Count;
    }

    public ScrewColor GetNextColor()
    {
        if (!HasNextBox())
            return ScrewColor.Gray;

        return boxColors[currentColorIndex++];
    }

}