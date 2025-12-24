 using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;   

public class UIGameplayController : GUIBase,MMEventListener<ReleaseScrew>,MMEventListener<DataChange>
{
    [SerializeField] public TextMeshProUGUI playerCoinText;
    [SerializeField] public TextMeshProUGUI screwCountText;
    [SerializeField] public TextMeshProUGUI levelText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        this.MMEventStartListening<ReleaseScrew>();
        this.MMEventStartListening<DataChange>();
    }

    private void OnDestroy()
    {
        this.MMEventStopListening<ReleaseScrew>();
        this.MMEventStopListening<DataChange>();
    }

    void Start()
    {
        playerCoinText.text = DataManager.Instance.PlayerData.Coin.ToString();
        screwCountText.text = "0";
        levelText.text = "Level " + DataManager.Instance.PlayerData.CurrentLevelIndex.ToString();
    }

    public void OnMMEvent(ReleaseScrew eventType)
    {
        screwCountText.text = DataManager.Instance.SessionData.screwsRemoved.ToString();
    }
 

    public void OnMMEvent(DataChange eventType)
    {
        playerCoinText.text = DataManager.Instance.PlayerData.Coin.ToString();
        levelText.text = "Level " + DataManager.Instance.PlayerData.CurrentLevelIndex.ToString();
    }
}
