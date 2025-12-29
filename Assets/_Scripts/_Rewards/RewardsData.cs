using System.Collections.Generic;
public enum RewardSource
{
    LevelComplete,
    DailyLogin,
    Event,
    AdBonus
}
public enum RewardType
{
    Coin,
    Life,
    Drill,
    Broom,
    Hammer,
    Magnet
}


public class RewardData
{
    public RewardSource source;
    public int level;
    public Dictionary<RewardType, int> rewards;
    public RewardData(RewardSource src, int lvl = -1)
    {
        source = src;
        level = lvl;
        rewards = new Dictionary<RewardType, int>();
    }

    public void AddReward(RewardType type, int amount)
    {
        if (!rewards.ContainsKey(type))
            rewards[type] = 0;

        rewards[type] += amount;
    }
    
}
