using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class LevelManager : MMSingleton<LevelManager>
{
    [Header("Builder")]
    public LevelBuilder builder;

    [Header("Levels")]
    [Tooltip("JSON đặt trong Resources/Levels")]
    public int totalLevels = 100;

    private int currentLevelIndex;
    private LevelRuntimeData currentLevel;

    // ======================
    // PUBLIC API
    // ======================

    public void StartLevel(int index)
    {
        currentLevelIndex = Mathf.Max(1, index);
        LoadLevel(currentLevelIndex);
    }


    public void NextLevel()
    {
        StartLevel(currentLevelIndex + 1);
    }

    public List<ScrewColor> GetSolution()
    {
        return currentLevel != null
            ? currentLevel.solutionColors
            : null;
    }

    // ======================
    // INTERNAL
    // ======================

    private void LoadLevel(int levelIndex)
    {
        ClearCurrentLevel();

        // 1. Load JSON
        TextAsset jsonAsset = Resources.Load<TextAsset>(
            $"Levels/level_{levelIndex:00}"
        );

        if (jsonAsset == null)
        {
            Debug.LogError($"[LevelManager] Không tìm thấy level {levelIndex}");
            return;
        }

        Debug.Log($"[LevelManager] Load {jsonAsset.name}");

        // 2. JSON → C# data
        JsonLevelData jsonData =
            JsonUtility.FromJson<JsonLevelData>(jsonAsset.text);

        // 3. C# → Runtime data
        currentLevel = new LevelRuntimeData(jsonData);

        // 4. Build
        builder.Build(currentLevel);
        MMEventManager.TriggerEvent(
            new LevelSolutionReadyEvent(currentLevel.solutionColors)
        );
    }

    private void ClearCurrentLevel()
    {
        if (builder != null)
        {
            builder.CleanupPreviousBuild();
        }

        currentLevel = null;
    }
}