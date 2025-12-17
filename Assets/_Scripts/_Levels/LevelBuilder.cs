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
    // Sửa lại đoạn code trong hàm BuildLevel() của LevelBuilder
private void BuildLevel()
{
    int dimX = 0;
    int dimY = 0;
    int dimZ = 0;

    if (levelData == null || levelData.levelLayers == null || levelData.plankModule == null)
    {
        Debug.LogError("Cần gán Level Data và đảm bảo Level Layers không rỗng, và Plank Module phải được gán.");
        return;
    }
    
    CleanupPreviousBuild();

    GameObject buildContainer = new GameObject(BuildContainerName);
    buildContainer.transform.SetParent(this.transform);
    
    // Lấy dữ liệu và prefab
    List<Layer> matrix = levelData.levelLayers;
    GameObject baseModulePrefab = levelData.plankModule; // Chỉ là 1 GameObject

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
            // Thay đổi: columns bây giờ là List<CubeBlock>
            List<CubeBlock> columns = rows[y].columns; 
            int currentDimX = columns.Count; // Kích thước X của hàng hiện tại
            if (z == 0 && y == 0) dimX = currentDimX; // Lưu kích thước X từ hàng đầu tiên

            for (int x = 0; x < currentDimX; x++) // Cột X (Width)
            {
                // Thay đổi: Lấy thông tin khối tại vị trí (x, y, z)
                CubeBlock currentBlock = columns[x];
                
                // Điều kiện sinh khối mới: Nếu danh sách vít rỗng/null, thì coi như là khối trống
                if (currentBlock.screws == null || currentBlock.screws.Count == 0) continue; 
                
                // Khối cơ bản luôn là plankModule
                GameObject prefabToSpawn = baseModulePrefab; 

                // 1. Tính toán vị trí
                float xPos = x * cubeSize;
                float yPos = y * cubeSize;
                float zPos = z * cubeSize;
                Vector3 spawnPosition = new Vector3(xPos, yPos, zPos);

                GameObject cubeInstance = null;
                
                // 2. Sinh khối
#if UNITY_EDITOR
                cubeInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToSpawn, buildContainer.transform);
                
                cubeInstance.transform.localPosition = spawnPosition;
                cubeInstance.name = $"{prefabToSpawn.name}_{x}_{y}_{z}";
                
                Undo.RegisterCreatedObjectUndo(cubeInstance, "Create Cube Instance");
#else
                // Nếu không ở Editor, dùng Instantiate thông thường
                cubeInstance = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, buildContainer.transform);
#endif

                // 3. Khởi tạo đối tượng Cube và thiết lập các vít
                if (cubeInstance != null)
                {
                    // Lấy Component CubeController trên khối vừa sinh ra
                    CubeController controller = cubeInstance.GetComponent<CubeController>(); 

                    if (controller != null)
                    {
                        // Truyền thông tin vít (bao gồm hướng và màu) cho Controller để nó tự thiết lập
                        controller.Initialize(currentBlock.screws); 
                    }
                    else
                    {
                        Debug.LogWarning($"Khối {cubeInstance.name} thiếu CubeController. Không thể thiết lập vít.");
                    }
                    
                    // (Optional) Khởi tạo đối tượng Cube class để lưu trữ runtime data nếu cần
                    // Cube cubeData = new Cube(xPos, yPos, zPos, currentBlock.screws.Select(s => (int)s.direction).ToList()); 
                }
            }
        }
    }
    // Dòng Debug.Log hiện tại đã có thể truy cập các biến kích thước
    Debug.Log($"Level Built Successfully with dimensions {dimX}x{dimY}x{dimZ} layers.");
    // ===========================================
// 4. CANH GIỮA BUILD CONTAINER VỀ TÂM (0,0,0)
// ===========================================
    Vector3 centerOffset = new Vector3(
        (dimX - 1) * cubeSize * 0.5f,
        (dimY - 1) * cubeSize * 0.5f,
        (dimZ - 1) * cubeSize * 0.5f
    );

// Đưa buildContainer về tâm
    buildContainer.transform.localPosition = -centerOffset;

    Debug.Log("Level Centered at " + buildContainer.transform.localPosition);

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