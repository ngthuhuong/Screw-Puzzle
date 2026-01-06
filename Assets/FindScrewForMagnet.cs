using System.Collections.Generic;
using UnityEngine;

public class FindScrewForMagnet : MonoBehaviour
{
    private List<ScrewController> list;
    
    private void Start()
    {
        list = new List<ScrewController>();
    }
    
    public List<ScrewController> FindAllModelScrews(ScrewColor color, int num)
    {
        Debug.Log("[FindScrewForMagnet] Method called, num = " + num);
        list.Clear();
        ScrewController[] allScrews = FindObjectsOfType<ScrewController>();
        int count = 0;
        foreach (var screw in allScrews)
        {
            if(count >= num) break;
            if (screw.IsRemoved) continue;
            if (screw.GetColor() != color) continue;
            list.Add(screw);
            count++;
            Debug.Log("Found screw: " + screw.name);
        }

        return list;
    }
}
