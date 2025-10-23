using UnityEngine;
using System.Collections.Generic;

// Đảm bảo bạn có thư mục 'Editor' để chứa các thư viện UnityEditor
#if UNITY_EDITOR 
using UnityEditor; 
#endif

public class LevelBuilder : MonoBehaviour
{
    public float cubeSize = 2.47f;
    
    public LevelData levelData; 
    
    private const string BuildContainerName = "--- GENERATED CUBES ---";

    [ContextMenu("BUILD_LEVEL_IN_EDITOR")]
    private void BuildLevel()
    {
        // Khai báo các biến kích thước ở phạm vi của hàm
        int dimX = 0;
        int dimY = 0;
        int dimZ = 0;

        if (Application.isPlaying)
        {
            Debug.LogWarning("Không thể Build Level khi đang ở Play Mode.");
            return;
        }

        if (levelData == null || levelData.plankModules == null || levelData.levelLayers == null)
        {
            Debug.LogError("Cần gán Level Data và đảm bảo Level Layers không rỗng.");
            return;
        }
        
        CleanupPreviousBuild();

        GameObject buildContainer = new GameObject(BuildContainerName);
        buildContainer.transform.SetParent(this.transform);
        
        List<Layer> matrix = levelData.levelLayers;
        GameObject[] modules = levelData.plankModules;

        // Tính kích thước Z
        dimZ = matrix.Count; 

        // DUYỆT VÀ SINH CUBES (Duyệt cấu trúc List lồng nhau)
        for (int z = 0; z < dimZ; z++) // Lớp Z (Depth)
        {
            List<Row> rows = matrix[z].rows;
            int currentDimY = rows.Count; // Kích thước Y của lớp hiện tại
            if (z == 0) dimY = currentDimY; // Lưu kích thước Y từ lớp đầu tiên

            for (int y = 0; y < currentDimY; y++) // Lớp Y (Height)
            {
                List<int> columns = rows[y].columns;
                int currentDimX = columns.Count; // Kích thước X của hàng hiện tại
                if (z == 0 && y == 0) dimX = currentDimX; // Lưu kích thước X từ hàng đầu tiên

                for (int x = 0; x < currentDimX; x++) // Cột X (Width)
                {
                    int moduleIndex = columns[x];
                    
                    if (moduleIndex <= 0) continue; 
                    
                    if (moduleIndex - 1 >= modules.Length || modules[moduleIndex - 1] == null)
                    {
                        Debug.LogWarning($"Level Data lỗi: Module index {moduleIndex} không hợp lệ.");
                        continue;
                    }

                    GameObject prefabToSpawn = modules[moduleIndex - 1];

                    float xPos = x * cubeSize;
                    float yPos = y * cubeSize;
                    float zPos = z * cubeSize;
                    Vector3 spawnPosition = new Vector3(xPos, yPos, zPos);

#if UNITY_EDITOR
                    GameObject cubeInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToSpawn, buildContainer.transform);
                    
                    cubeInstance.transform.localPosition = spawnPosition;
                    cubeInstance.name = $"{prefabToSpawn.name}_{x}_{y}_{z}";
                    
                    Undo.RegisterCreatedObjectUndo(cubeInstance, "Create Cube Instance");
#endif
                }
            }
        }
        // Dòng Debug.Log hiện tại đã có thể truy cập các biến kích thước
        Debug.Log($"Level Built Successfully with dimensions {dimX}x{dimY}x{dimZ} layers.");
    }
    
    // Hàm này dọn dẹp tất cả các khối đã sinh ra trước đó
    private void CleanupPreviousBuild()
    {
#if UNITY_EDITOR
        Transform previousBuild = transform.Find(BuildContainerName);
        
        if (previousBuild != null)
        {
            Undo.DestroyObjectImmediate(previousBuild.gameObject);
        }
#endif
    }
}