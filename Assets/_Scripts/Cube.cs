using System.Collections.Generic;
using UnityEngine;

public class CubeSaving : MonoBehaviour
{
  
}

public class Cube
{
    public float x, y, z;
    public int screwNumber;
    public List<int> screwDir;
    public Cube(float x, float y, float z, List<int> screwDir)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.screwDir = screwDir;
        screwNumber = screwDir.Count;
    }
    public Cube(float x, float y, float z,int top=0, int bottom=0, int front=0, int back=0, int right=0, int left=0)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        screwDir = new List<int>();
        if (top != 0) screwDir.Add(top);
        if (bottom != 0) screwDir.Add(bottom);
        if (front != 0) screwDir.Add(front);
        if (back != 0) screwDir.Add(back);
        if (right != 0) screwDir.Add(right);
        if (left != 0) screwDir.Add(left);
        screwNumber = screwDir.Count;
    }
}