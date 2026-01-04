using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class RewardManager : MMSingleton<RewardManager>
{
    private List<RewardData> rewards = new List<RewardData>();
    
    [Header("Reward Prefabs")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject lifePrefab;
    [SerializeField] private GameObject drillPrefab;
    [SerializeField] private GameObject broomPrefab;
    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private GameObject magnetUI;
    [SerializeField] private GameObject magnetGO;


    protected override void Awake()
    {
        base.Awake();
        ReadRewardData();
    }

    private void ReadRewardData()
    {
        var lv1 = new RewardData(RewardSource.LevelComplete, 1);
        lv1.AddReward(RewardType.Coin, 10);
        rewards.Add(lv1);

        var lv2 = new RewardData(RewardSource.LevelComplete, 2);
        lv2.AddReward(RewardType.Coin, 10);
        lv2.AddReward(RewardType.Drill, 1);
        rewards.Add(lv2);

        var lv3 = new RewardData(RewardSource.LevelComplete, 3);
        lv3.AddReward(RewardType.Coin, 10);
        rewards.Add(lv3);

        var lv5 = new RewardData(RewardSource.LevelComplete, 5);
        lv5.AddReward(RewardType.Coin, 50);
        lv5.AddReward(RewardType.Drill, 1);
        rewards.Add(lv5);

        // ===== Daily Login =====
        var daily = new RewardData(RewardSource.DailyLogin);
        daily.AddReward(RewardType.Coin, 50);
        rewards.Add(daily);

        var ad = new RewardData(RewardSource.AdBonus);
        ad.AddReward(RewardType.Coin, 150);
        rewards.Add(ad);
    }

    public List<RewardData> GetRewards(RewardSource source, int level)
    {
        List<RewardData> result = new();

        foreach (var r in rewards)
        {
            if (r.source != source) continue;
            if (r.level != -1 && r.level != level) continue;

            result.Add(r);
        }

        return result;
    }
    public GameObject GetRewardPrefab(RewardType type)
    {
        return type switch
        {
            RewardType.Coin => coinPrefab,
            RewardType.Life => lifePrefab,
            RewardType.Drill => drillPrefab,
            RewardType.Broom => broomPrefab,
            RewardType.Hammer => hammerPrefab,
            RewardType.Magnet => magnetUI,
            _ => null
        };
    }
    
    public void LoadRewardsAsChildren(
        RewardSource source,
        int level,
        Transform parent)
    {
        ClearChildren(parent);

        var rewardDatas = GetRewards(source, level);

        foreach (var data in rewardDatas)
        {
            foreach (var pair in data.rewards)
            {
                RewardType type = pair.Key;
                int amount = pair.Value;

                var prefab = GetRewardPrefab(type);
                if (prefab == null)
                {
                    Debug.LogWarning($"No prefab for reward type {type}");
                    continue;
                }

                var go = Instantiate(prefab, parent);

                var view = go.GetComponent<RewardItemView>();
                if (view != null)
                {
                    view.SetupAsReward(type, amount);
                    Debug.Log("Loaded reward: " + type + " x" + amount);
                }
            }

        }
    }
    private void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
    public int GetTotalCoin(RewardSource source, int level)
    {
        int total = 0;

        var rewardDatas = GetRewards(source, level);

        foreach (var data in rewardDatas)
        {
            if (data.rewards.TryGetValue(RewardType.Coin, out int amount))
            {
                total += amount;
            }
        }

        return total;
    }


    #region Use Tools

    public void UseTool(RewardType type)
    {
        DataManager.Instance.PlayerData.UseToolATime(type);
        switch (type)
        {
            case RewardType.Drill:
                UseDrill();
                break;
            case RewardType.Broom:
                UseBroom();
                break;
            case RewardType.Hammer:
                UseHammer();
                break;
            case RewardType.Magnet:
                UseMagnet();
                break;
            default:
                Debug.LogError($"Unknown reward type {type}");
                break;
            
        }
        
    }

    private void UseDrill()
    {
        Debug.Log("Drill used!");
        MMEventManager.TriggerEvent(new UseDrillTool());
    }
    
    private void UseHammer()
    {
        Debug.Log("UseHammer used!");
        MMEventManager.TriggerEvent(new UseHammerTool());
    }
    private void UseMagnet()
    {
        Debug.Log("Manet used!");
        
        MMEventManager.TriggerEvent(new UseMagnetTool());
    }
    private void UseBroom()
    {
        Debug.Log("Broom used!");
        MMEventManager.TriggerEvent(new UseBroomTool());
    }

    #endregion
}