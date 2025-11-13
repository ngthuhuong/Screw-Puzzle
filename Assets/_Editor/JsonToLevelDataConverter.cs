using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq; // Cần cho hàm Select

#if UNITY_EDITOR 
// Lưu ý: Các cấu trúc Json... được định nghĩa ở trên (bước 2)

public class JsonToLevelDataConverter
{
    [MenuItem("Tools/Level Builder/Import JSON to LevelData")]
    public static void ImportJson()
    {
        // 1. Chọn file JSON
        string jsonPath = EditorUtility.OpenFilePanel("Chọn File JSON Level", Application.dataPath, "json");

        if (string.IsNullOrEmpty(jsonPath))
        {
            Debug.Log("Hủy bỏ việc nhập file JSON.");
            return;
        }

        // 2. Chọn LevelData ScriptableObject để ghi đè dữ liệu
        LevelData targetLevelData = Selection.activeObject as LevelData;
        if (targetLevelData == null)
        {
            EditorUtility.DisplayDialog("Lỗi", "Bạn phải chọn một đối tượng LevelData trong Project Panel trước khi chạy công cụ này.", "OK");
            return;
        }

        // Bắt đầu quá trình Undo để có thể hoàn tác
        Undo.RecordObject(targetLevelData, "Import JSON Data to LevelData");

        try
        {
            // 3. Đọc và Deserialize JSON
            string jsonString = File.ReadAllText(jsonPath);
            JsonLevelStructure jsonStructure = JsonUtility.FromJson<JsonLevelStructure>(jsonString);

            if (jsonStructure == null || jsonStructure.layers == null)
            {
                EditorUtility.DisplayDialog("Lỗi", "File JSON không hợp lệ hoặc thiếu cấu trúc 'layers'.", "OK");
                return;
            }

            // 4. Chuyển đổi từ JsonStructure sang LevelData
            List<Layer> newLevelLayers = new List<Layer>();

            foreach (var jsonLayer in jsonStructure.layers)
            {
                Layer newLayer = new Layer();
                newLayer.rows = new List<Row>();

                foreach (var jsonRow in jsonLayer.rows)
                {
                    Row newRow = new Row();
                    newRow.columns = new List<CubeBlock>();

                    foreach (var jsonCubeBlock in jsonRow.columns)
                    {
                        CubeBlock newBlock = new CubeBlock();
                        newBlock.screws = new List<ScrewInfo>();

                        // Chuyển đổi danh sách JsonScrewInfo sang List<ScrewInfo>
                        if (jsonCubeBlock.screws != null)
                        {
                            newBlock.screws = jsonCubeBlock.screws.Select(jsonScrew => new ScrewInfo
                            {
                                // Ép kiểu số nguyên sang Enum
                                direction = (ScrewFace)jsonScrew.direction,
                                color = (ScrewColor)jsonScrew.color
                            }).ToList();
                        }
                        
                        newRow.columns.Add(newBlock);
                    }
                    newLayer.rows.Add(newRow);
                }
                newLevelLayers.Add(newLayer);
            }

            // 5. Gán dữ liệu đã chuyển đổi vào LevelData
            targetLevelData.levelLayers = newLevelLayers;
            
            // Đánh dấu LevelData là đã thay đổi để Unity lưu lại
            EditorUtility.SetDirty(targetLevelData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"✅ Thành công: Đã nhập dữ liệu từ JSON vào LevelData '{targetLevelData.name}'!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Lỗi khi chuyển đổi JSON: {e.Message}");
            EditorUtility.DisplayDialog("Lỗi", $"Lỗi xảy ra trong quá trình xử lý JSON: {e.Message}", "OK");
        }
    }
}
#endif