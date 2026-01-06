using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIMenuController : GUIBase
{
    [SerializeField]
    public Button playButton;
    public Button playerProfileButton;
    private TextMeshProUGUI levelText;

    public TextMeshProUGUI playerCoin;
    public TextMeshProUGUI playerHeart;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(HandlePlayButton);
            levelText = playButton.GetComponentInChildren<TextMeshProUGUI>();
            levelText.text = "Play Level " + DataManager.Instance.PlayerData.CurrentLevelIndex.ToString();
        }
        else
        {
            Debug.LogError("Play button null");
        }
        
        playerCoin.text = DataManager.Instance.PlayerData.Coin.ToString();
        playerHeart.text = DataManager.Instance.PlayerData.Lives.ToString();
    }

    private void OnEnable()
    {
        if (levelText == null)
        {
            levelText = playButton.GetComponentInChildren<TextMeshProUGUI>();
        }
        levelText.text = "Play Level " + DataManager.Instance.PlayerData.CurrentLevelIndex.ToString();
    }


    private void HandlePlayButton()
    {
        Debug.Log("Play clicked, lives = " + DataManager.Instance.PlayerData.Lives);

        if (DataManager.Instance.PlayerData.Lives <= 0)
        {
            GUIManager.Instance.ShowMarketingPanel();
            return;
        }

        GameManager.Instance.StartGame();
    }
    public void UpdatePlayerResources()
    {
        playerCoin.text = DataManager.Instance.PlayerData.Coin.ToString();
        playerHeart.text = DataManager.Instance.PlayerData.Lives.ToString();
    }
}
