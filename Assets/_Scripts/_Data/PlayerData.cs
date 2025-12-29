using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class ToolData
{
    public RewardType type;
    public int amount;
}
public class PlayerData
{
    private int coin;
    private int lives;
    private int currentLevelIndex;
    private List<ToolData> tools = new List<ToolData>();

    public int Coin => coin;
    public int Lives => lives;
    public int CurrentLevelIndex => currentLevelIndex;

    private const string COIN_KEY = "player_coin";
    private const string LIVES_KEY = "player_lives";
    private const string CURRENT_LEVEL_KEY = "current_level";
    private const string TOOLS_KEY = "player_tools";

    #region Load / Save

    public void Load()
    {
        coin = PlayerPrefs.GetInt(COIN_KEY, 0);
        lives = PlayerPrefs.GetInt(LIVES_KEY, 3);
        currentLevelIndex = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 1);

        LoadTools();
    }

    public void Save()
    {
        PlayerPrefs.SetInt(COIN_KEY, coin);
        PlayerPrefs.SetInt(LIVES_KEY, lives);
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentLevelIndex);

        SaveTools();

        PlayerPrefs.Save();
        MMEventManager.TriggerEvent(new DataChange());
    }

    public void LoadDefaults()
    {
        coin = 0;
        lives = 10;
        currentLevelIndex = 1;

        if (tools == null)
            tools = new List<ToolData>();
        else
            tools.Clear();

        // ===== TEST DATA =====
        tools.Add(new ToolData
        {
            type = RewardType.Drill,
            amount = 3
        });

        tools.Add(new ToolData
        {
            type = RewardType.Hammer,
            amount = 2
        });

        tools.Add(new ToolData
        {
            type = RewardType.Broom,
            amount = 5
        });
        
        Save();
    }


    #endregion

    #region Tool Save / Load (JSON)

    private void SaveTools()
    {
        string json = JsonUtility.ToJson(new ToolWrapper { tools = tools });
        PlayerPrefs.SetString(TOOLS_KEY, json);
    }

    private void LoadTools()
    {
        if (!PlayerPrefs.HasKey(TOOLS_KEY))
        {
            tools = new List<ToolData>();
            return;
        }

        string json = PlayerPrefs.GetString(TOOLS_KEY);
        ToolWrapper wrapper = JsonUtility.FromJson<ToolWrapper>(json);
        tools = wrapper != null ? wrapper.tools : new List<ToolData>();
    }

    [System.Serializable]
    private class ToolWrapper
    {
        public List<ToolData> tools;
    }

    #endregion

    #region Coin / Life

    public void AddCoin(int amount)
    {
        coin += amount;
        Save();
    }

    public bool UseCoin(int amount)
    {
        if (coin < amount) return false;

        coin -= amount;
        Save();
        return true;
    }

    public void LoseLife()
    {
        if (lives > 0)
            lives--;

        Save();
    }

    public void ResetLives(int newLives = 3)
    {
        lives = newLives;
        Save();
    }

    #endregion

    #region Level

    public void SetNextLevel()
    {
        currentLevelIndex++;
        Save();
    }

    #endregion

    #region Tool Inventory

    public void AddTool(RewardType type, int amount)
    {
        var tool = tools.Find(t => t.type == type);
        if (tool != null)
        {
            tool.amount += amount;
        }
        else
        {
            tools.Add(new ToolData
            {
                type = type,
                amount = amount
            });
        }

        Save();
    }

    public int GetToolAmount(RewardType type)
    {
        var tool = tools.Find(t => t.type == type);
        return tool != null ? tool.amount : 0;
    }

    public List<ToolData> GetTools()
    {
        return tools;
    }
    public bool HasTool(RewardType type)
    {
        var tool = tools.Find(t => t.type == type);
        return tool != null ? true : false;
    }
    public void UseToolATime(RewardType type)
    {
        var tool = tools.Find(t => t.type == type);
        if (tool != null && tool.amount > 0)
        {
            tool.amount--;
            Save();
        }
    }
    #endregion

    #region Reward Apply

    public void ApplyReward(ToolData reward)
    {
        switch (reward.type)
        {
            case RewardType.Coin:
                AddCoin(reward.amount);
                break;

            case RewardType.Life:
                lives += reward.amount;
                Save();
                break;

            default:
                AddTool(reward.type, reward.amount);
                break;
        }
    }

    #endregion
}
