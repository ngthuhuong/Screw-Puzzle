using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class LoadBoxes : MonoBehaviour,MMEventListener<LevelSolutionReadyEvent>
{
    [Header("Prefab box")]
    public StorageBox boxPrefab;
    public List<ScrewColor> boxColors;
    private int currentColorIndex = 0;
    private StorageBox currentBox;
    
    public StorageBox LoadNextBox(Vector3 replacePosition, Quaternion replaceRotation)
    {
        if (boxColors == null || boxColors.Count == 0)
        {
            Debug.LogWarning("[LoadBoxes] LevelData chưa có danh sách màu!");
            return null;
        }

        ScrewColor nextColor = boxColors[currentColorIndex];
        Vector3 spawnPos = replacePosition + Vector3.up * 5f;
        StorageBox newBox = Instantiate(boxPrefab, spawnPos, replaceRotation, transform);
        newBox.name = $"StorageBox_{nextColor}";
        newBox.SetColor(nextColor);

        newBox.StartCoroutine(newBox.MoveDownTo(replacePosition));

        currentColorIndex = (currentColorIndex + 1) % boxColors.Count;

        Debug.Log($"[LoadBoxes] Sinh box mới: {newBox.name} (màu {nextColor}) tại {replacePosition}");

        return newBox;
    }
    public ScrewColor GetNextColor()
    {
        if ( boxColors == null || boxColors.Count == 0)
        {
            Debug.LogWarning("[LoadBoxes] LevelData chưa có danh sách màu!");
            return default;
        }

        ScrewColor nextColor = boxColors[currentColorIndex];
        currentColorIndex = (currentColorIndex + 1) % boxColors.Count;
        return nextColor;
    }

    public void OnMMEvent(LevelSolutionReadyEvent e)
    {
        boxColors = e.solutionColors;
        currentColorIndex = 0;
    }
    public void SetSolution(List<ScrewColor> solution)
    {
        boxColors = solution;
        currentColorIndex = 0;
    }

    private void OnEnable()
    {
        this.MMEventStartListening<LevelSolutionReadyEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<LevelSolutionReadyEvent>();
    }
}