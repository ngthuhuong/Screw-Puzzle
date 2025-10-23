using UnityEngine;
using System.Collections.Generic;
using System; 

[Serializable]
public struct Row
{
    public List<int> columns; 
}

[Serializable]
public struct Layer
{
    public List<Row> rows; 
}

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public GameObject[] plankModules; 

    // 3. Sử dụng List lồng nhau cho 3D (Z, Y, X)
    [Tooltip("Cấu trúc Level: List<Layer> (Z-Axis)")]
    public List<Layer> levelLayers; 

}