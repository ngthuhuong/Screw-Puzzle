using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;   

public class UIGameplayController : GUIBase,MMEventListener<ReleaseScrew>
{
    [SerializeField] public TextMeshProUGUI playerCoinText;
    [SerializeField] public TextMeshProUGUI screwCountText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        this.MMEventStartListening<ReleaseScrew>();
    }

    private void OnDestroy()
    {
        this.MMEventStopListening<ReleaseScrew>();
    }

    void Start()
    {
        playerCoinText.text = DataManager.Instance.PlayerData.Coin.ToString();
        screwCountText.text = "0";
        //+ GameManager.Instance.LevelData.totalScrews.ToString();
    }

    public void OnMMEvent(ReleaseScrew eventType)
    {
        screwCountText.text = DataManager.Instance.SessionData.screwsRemoved.ToString();
    }
}
