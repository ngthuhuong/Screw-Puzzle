using MoreMountains.Tools;
using UnityEngine;

public class LevelManager : MMSingleton<LevelManager>
{
    public LevelBuilder builder;
    public TextAsset[] levelJsonFiles;

    private int currentLevelIndex;

    public void StartLevel(int index)
    {
        currentLevelIndex = index;

        LevelRuntimeData data =
            LevelJsonLoader.LoadFromJson(levelJsonFiles[index]);

        builder.Build(data);
        Debug.Log(levelJsonFiles[index].name);
    }

    public void NextLevel()
    {
        StartLevel(currentLevelIndex + 1);
    }
}