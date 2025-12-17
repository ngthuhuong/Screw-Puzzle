using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public static class LevelJsonLoader
{
    public static LevelRuntimeData LoadFromJson(TextAsset jsonAsset)
    {
        JsonLevelStructure json = JsonUtility.FromJson<JsonLevelStructure>(jsonAsset.text);

        LevelRuntimeData runtimeData = new LevelRuntimeData();
        runtimeData.levelLayers = Convert(json);

        return runtimeData;
    }

    private static List<Layer> Convert(JsonLevelStructure json)
    {
        List<Layer> layers = new List<Layer>();

        foreach (var jl in json.layers)
        {
            Layer layer = new Layer();
            layer.rows = new List<Row>();

            foreach (var jr in jl.rows)
            {
                Row row = new Row();
                row.columns = new List<CubeBlock>();

                foreach (var jc in jr.columns)
                {
                    CubeBlock block = new CubeBlock();
                    block.screws = jc.screws?.Select(s => new ScrewInfo
                    {
                        direction = (ScrewFace)s.direction,
                        color = (ScrewColor)s.color
                    }).ToList();

                    row.columns.Add(block);
                }
                layer.rows.Add(row);
            }
            layers.Add(layer);
        }
        return layers;
    }
}

