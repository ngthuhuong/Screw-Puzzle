using System;
using System.Collections.Generic;
using UnityEngine;

// Đặt trong file LevelDataConverter.cs
[Serializable]
public class JsonScrewInfo
{
    public int direction; // Sẽ được chuyển đổi sang ScrewFace
    public int color;     // Sẽ được chuyển đổi sang ScrewColor
}

[Serializable]
public class JsonCubeBlock
{
    public List<JsonScrewInfo> screws;
}

[Serializable]
public class JsonRow
{
    public List<JsonCubeBlock> columns;
}

[Serializable]
public class JsonLayer
{
    public List<JsonRow> rows;
}

[Serializable]
public class JsonLevelStructure // Lớp container chứa toàn bộ cấu trúc
{
    public List<JsonLayer> layers;
}