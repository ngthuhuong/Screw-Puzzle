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
}