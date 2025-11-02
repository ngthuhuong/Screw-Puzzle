using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIMenuController : GUIBase
{
    [SerializeField]
    public Button playButton;
    public Button playerProfileButton;

    public TextMeshProUGUI playerCoin;
    public TextMeshProUGUI playerHeart;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(HandlePlayButton);
        playerCoin.text = DataManager.Instance.PlayerData.Coin.ToString();
        playerHeart.text = DataManager.Instance.PlayerData.Lives.ToString();
    }


    private void HandlePlayButton()
    {
        Debug.Log("Play clicked, lives = " + DataManager.Instance.PlayerData.Lives);

        if (DataManager.Instance.PlayerData.Lives <= 0)
        {
            GUIManager.Instance.ShowMarketingPanel();
            return;
        }

        GameManager.Instance.ResumeGame();
        
    }
    public void UpdatePlayerResources()
    {
        playerCoin.text = DataManager.Instance.PlayerData.Coin.ToString();
        playerHeart.text = DataManager.Instance.PlayerData.Lives.ToString();
    }
}
