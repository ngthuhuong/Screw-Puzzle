using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelBuilder : MonoBehaviour
{
    public float cubeSize = 2.47f;

    [Header("Runtime Config")]
    [SerializeField] private GameObject plankModulePrefab;

    private const string BuildContainerName = "--- GENERATED CUBES ---";

    private Transform buildContainer;

    // ================================
    // RUNTIME BUILD ENTRY POINT
    // ================================
    public void Build(LevelRuntimeData runtimeData)
    {
        if (runtimeData == null || runtimeData.levelLayers == null)
        {
            Debug.LogError("RuntimeData hoặc levelLayers rỗng.");
            return;
        }

        if (plankModulePrefab == null)
        {
            Debug.LogError("Chưa gán plankModulePrefab.");
            return;
        }

        CleanupPreviousBuild();

        buildContainer = new GameObject(BuildContainerName).transform;
        buildContainer.SetParent(transform);

        BuildInternal(runtimeData.levelLayers);
    }
    
    private void BuildInternal(List<Layer> matrix)
    {
        int dimX = 0, dimY = 0, dimZ = matrix.Count;

        for (int z = 0; z < dimZ; z++)
        {
            List<Row> rows = matrix[z].rows;
            int currentDimY = rows.Count;
            if (z == 0) dimY = currentDimY;

            for (int y = 0; y < currentDimY; y++)
            {
                List<CubeBlock> columns = rows[y].columns;
                int currentDimX = columns.Count;
                if (z == 0 && y == 0) dimX = currentDimX;

                for (int x = 0; x < currentDimX; x++)
                {
                    CubeBlock currentBlock = columns[x];

                    if (currentBlock.screws == null || currentBlock.screws.Count == 0)
                        continue;

                    Vector3 spawnPosition = new Vector3(
                        x * cubeSize,
                        y * cubeSize,
                        z * cubeSize
                    );

                    GameObject cubeInstance;

#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        cubeInstance = (GameObject)PrefabUtility.InstantiatePrefab(
                            plankModulePrefab, buildContainer
                        );
                    }
                    else
                    {
                        cubeInstance = Instantiate(
                            plankModulePrefab,
                            spawnPosition,
                            Quaternion.identity,
                            buildContainer
                        );
                    }
#else
cubeInstance = Instantiate(
    plankModulePrefab,
    spawnPosition,
    Quaternion.identity,
    buildContainer
);
#endif


                    cubeInstance.name = $"{plankModulePrefab.name}_{x}_{y}_{z}";

                    CubeController controller = cubeInstance.GetComponent<CubeController>();
                    if (controller != null)
                    {
                        controller.Initialize(currentBlock.screws);
                    }
                    else
                    {
                        Debug.LogWarning($"{cubeInstance.name} thiếu CubeController.");
                    }
                }
            }
        }

        CenterLevel(dimX, dimY, dimZ);

        Debug.Log($"Level built: {dimX}x{dimY}x{dimZ}");
    }

 
    private void CenterLevel(int dimX, int dimY, int dimZ)
    {
        if (buildContainer == null) return;

        Renderer[] renderers = buildContainer.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        // Tâm hiện tại của mô hình (world)
        Vector3 modelCenterWorld = bounds.center;

        // Tâm mong muốn (world) = vị trí GameObject chứa LevelBuilder
        Vector3 targetCenterWorld = transform.position;

        // Độ lệch cần dịch
        Vector3 delta = targetCenterWorld - modelCenterWorld;

        // Dịch container
        buildContainer.position += delta;
    }



    // ================================
    // CLEAR
    // ================================
    public void CleanupPreviousBuild()
    {
#if UNITY_EDITOR
        Transform old = transform.Find(BuildContainerName);
        if (old != null)
        {
            Undo.DestroyObjectImmediate(old.gameObject);
            return;
        }
#endif

        Transform runtimeOld = transform.Find(BuildContainerName);
        if (runtimeOld != null)
        {
            Destroy(runtimeOld.gameObject);
        }
    }

    
}
