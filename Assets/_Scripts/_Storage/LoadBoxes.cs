using UnityEngine;

public class LoadBoxes : MonoBehaviour
{
    [Header("Prefab box")]
    public StorageBox boxPrefab;

    [Header("Dữ liệu Level hiện tại")]
    public LevelData levelData;

    private int currentColorIndex = 0;
    private StorageBox currentBox;

    /// <summary>
    /// Tạo box mới tại vị trí thay thế (xuất hiện trên đó 1f)
    /// </summary>
    public StorageBox LoadNextBox(Vector3 replacePosition, Quaternion replaceRotation)
    {
        if (levelData == null || levelData.boxColors == null || levelData.boxColors.Count == 0)
        {
            Debug.LogWarning("[LoadBoxes] LevelData chưa có danh sách màu!");
            return null;
        }

        ScrewColor nextColor = levelData.boxColors[currentColorIndex];
        Vector3 spawnPos = replacePosition + Vector3.up * 5f;
        StorageBox newBox = Instantiate(boxPrefab, spawnPos, replaceRotation, transform);
        newBox.name = $"StorageBox_{nextColor}";
        newBox.SetColor(nextColor);

        newBox.StartCoroutine(newBox.MoveDownTo(replacePosition));

        currentColorIndex = (currentColorIndex + 1) % levelData.boxColors.Count;

        Debug.Log($"[LoadBoxes] Sinh box mới: {newBox.name} (màu {nextColor}) tại {replacePosition}");

        return newBox;
    }

}