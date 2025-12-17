using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelRuntimeData
{
    public int levelId;
    public List<Layer> levelLayers;
    public List<ScrewColor> solutionColors;

    public LevelRuntimeData(JsonLevelData json)
    {
        levelId = json.levelId;
        levelLayers = json.grid.layers;

        solutionColors = json.solution.colors
            .Select(c => (ScrewColor)c)
            .ToList();
    }
}
