using UnityEngine;
using System.Collections.Generic;
using System; 
[Serializable]
public struct ScrewInfo
{
    [Tooltip("Hướng của vít (1=Top, 2=Bottom, ...)")]
    public ScrewFace direction;
    [Tooltip("Màu của vít")]
    public ScrewColor color;
}

[Serializable]
public struct CubeBlock // Đây sẽ thay thế cho 'int' cũ
{
    [Tooltip("Danh sách các vít trên khối này.")]
    public List<ScrewInfo> screws;
    
    // Nếu bạn muốn lưu loại khối (ví dụ: khối đặc, khối rỗng, v.v.), bạn có thể thêm:
    // public int blockType; 
}

[Serializable]
public struct Row
{
    // columns không còn là List<int> nữa
    public List<CubeBlock> columns; 
}

[Serializable]
public struct Layer
{
    public List<Row> rows; 
}

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    // Chỉ cần một Prefab khối cơ bản chứa 6 vít (tất cả đều bị disable ban đầu)
    public GameObject plankModule; 

    // Level Structure (Z, Y, X)
    public List<Layer> levelLayers; 
    
    // Danh sách màu bạn đã đề cập (có thể dùng Enum thay thế)
     public List<ScrewColor> boxColors; 
    
    // Nếu bạn muốn định nghĩa Prefab vít cho từng màu, có thể thêm:
    // public Dictionary<ScrewColor, GameObject> screwPrefabs;
}
[System.Serializable]
public class JsonLevelData
{
    public int levelId;
    public JsonGridData grid;
    public JsonSolutionData solution;
}
[System.Serializable]
public class JsonGridData
{
    public List<Layer> layers;
}
[System.Serializable]
public class JsonSolutionData
{
    public List<int> colors;
}
