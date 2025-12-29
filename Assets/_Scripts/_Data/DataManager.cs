using UnityEngine;
using MoreMountains.Tools;

public class DataManager : MMSingleton<DataManager>
{
    public PlayerData PlayerData { get; private set; }
    public SessionData SessionData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        LoadPlayerData();
        NewSession();
    }

    // ---------------- PLAYER DATA ----------------
    private void LoadPlayerData()
    {
        PlayerData = new PlayerData();
        PlayerData.LoadDefaults();
        //PlayerData.Load();
    }

    public void SavePlayerData()
    {
        PlayerData.Save();
    }

    // ---------------- SESSION DATA ----------------
    public void NewSession()
    {
        SessionData = new SessionData();
        SessionData.Enable();
    }

    public void EndSession()
    {
        SessionData = null;
    }

    // ---------------- LIFECYCLE ----------------
    protected virtual void OnApplicationQuit()
    {
        SavePlayerData();
    }

    public void ReceiveAllRewardsWinLevel()
    {
        var rewards = RewardManager.Instance.GetRewards(
            RewardSource.LevelComplete,
            PlayerData.CurrentLevelIndex
        );

        foreach (var rewardData in rewards)
        {
            foreach (var reward in rewardData.rewards)
            {
                PlayerData.ApplyReward(new ToolData
                {
                    type = reward.Key,
                    amount = reward.Value
                });
            }
        }
    }
    public void AddCoinReward(RewardSource source, int level)
    {
        int amount = RewardManager.Instance.GetTotalCoin(source, level);
        if (amount <= 0) return;
        PlayerData.AddCoin(amount);
    }
}