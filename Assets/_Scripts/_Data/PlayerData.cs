using UnityEngine;
using MoreMountains.Tools;

[System.Serializable]
public class PlayerData
{
    public int coin;
    public int lives;
    private int currentLevelIndex;

    public int Coin => coin;
    public int Lives => lives;
    public int CurrentLevelIndex => currentLevelIndex;
    
    private const string COIN_KEY = "player_coin";
    private const string LIVES_KEY = "player_lives";
    private const string CURRENT_LEVEL_KEY = "current_level";

    public void Load()
    {
        coin = PlayerPrefs.GetInt(COIN_KEY, 0);
        lives = PlayerPrefs.GetInt(LIVES_KEY, 3); // mặc định 3 mạng
        currentLevelIndex = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY, 0);
    }

    public void Save()
    {
        PlayerPrefs.SetInt(COIN_KEY, coin);
        PlayerPrefs.SetInt(LIVES_KEY, lives);
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentLevelIndex);
        PlayerPrefs.Save();
    }
    public void LoadDefaults()
    {
        coin = 0;
        lives = 3;
        currentLevelIndex = 0;
        Save();
    }
  

    public void AddCoin(int amount)
    {
        coin += amount;
        Save();
        MMEventManager.TriggerEvent(new DataChange());
    }

    public bool UseCoin(int amount)
    {
        if (coin >= amount)
        {
            coin -= amount;
            Save();
            return true;
        }
        return false;
    }

    public void LoseLife()
    {
        if (lives > 0) lives--;
        Debug.Log("-1 mang");
        Save();
        MMEventManager.TriggerEvent(new DataChange());

    }

    public void ResetLives(int newLives = 3)
    {
        lives = newLives;
        Save();
    }
}