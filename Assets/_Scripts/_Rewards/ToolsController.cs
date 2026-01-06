using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class ToolsController : MonoBehaviour,MMEventListener<DataChange>
{
    private void Start()
    {
        VisualizeTools();
    }
    private void OnEnable()
    {
        this.MMEventStartListening<DataChange>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<DataChange>();
    }
    //Clear all tools
    private void ClearAllTools()
    {
        Transform parent = this.transform;
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
    
    // Apply Tools to tool panel
    public void VisualizeTools()
    {
        if (DataManager.Instance == null) return;
        if (DataManager.Instance.PlayerData == null) return;

        var tools = DataManager.Instance.PlayerData.GetTools();
        if (tools == null) return;

        ClearAllTools();

        foreach (var tool in tools)
        {
            if (tool.amount <= 0)
                continue;
            var prefab = RewardManager.Instance.GetRewardPrefab(tool.type);
            if (prefab == null) continue;

            var go = Instantiate(prefab, transform);
            var view = go.GetComponent<RewardItemView>();
            if (view != null)
            {
                view.SetupInToolPannel(tool.type, tool.amount);
            }
        }
    }


    public void OnMMEvent(DataChange eventType)
    {
        VisualizeTools();
    }
}
