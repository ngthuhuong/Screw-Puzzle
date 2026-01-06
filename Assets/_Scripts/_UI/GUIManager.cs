using System;
using MoreMountains.Tools;
using UnityEngine;

public class GUIManager : MMSingleton<GUIManager>,MMEventListener<LoseGame>,MMEventListener<Confirm>,
    MMEventListener<DataChange>,MMEventListener<WinGameEvent>,MMEventListener<UseHammerTool>
{
    [SerializeField] private GUIMenuController mainMenu;
    [SerializeField] private UIGameplayController inGameUI;
    [SerializeField] private PopupController popupUI;
    [SerializeField] private MarketingPanel marketingPanel;
    [SerializeField] private AlertUIController alertUI;
    
    void Start()
    {
        if(mainMenu == null) mainMenu = GetComponentInChildren<GUIMenuController>();
        if(inGameUI == null) inGameUI = GetComponentInChildren<UIGameplayController>();
        if(popupUI == null) popupUI = GetComponentInChildren<PopupController>();
        if(marketingPanel == null) marketingPanel = GetComponentInChildren<MarketingPanel>();
        if(alertUI == null) alertUI = GetComponentInChildren<AlertUIController>();
        mainMenu.Show();
     
        popupUI.Hide();
        marketingPanel.Hide();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<LoseGame>();
        this.MMEventStartListening<Confirm>();
        this.MMEventStartListening<DataChange>();
        this.MMEventStartListening<WinGameEvent>();
        this.MMEventStartListening<UseHammerTool>();
    }
    private void OnDisable()
    {
        this.MMEventStopListening<LoseGame>();
        this.MMEventStopListening<Confirm>();
        this.MMEventStopListening<DataChange>();
        this.MMEventStopListening<WinGameEvent>();
        this.MMEventStopListening<UseHammerTool>();
    }

    //play game
    public void ShowInGameUI()
    {
        mainMenu.Hide();
        inGameUI.Show();
    }

    public void ShowMainMenu()
    {
        inGameUI.Hide();
        popupUI.Hide();
        marketingPanel.Hide();
        mainMenu.Show();
    }
    
    public void ShowPopup()
    {
        popupUI.Show();
    }

    public void onBackToMenuButton()
    {
        popupUI.EnableConfirmGroup( "Are you sure you want to return to the main menu?","BackToMenu");
    }

    public void onBackToMainMenuButtonAfterLose()
    {
        popupUI.EnableConfirmGroup("Xem quảng cáo để chơi  tiếp ?","RetryLevel");
    }
    public void OnNextLevelClicked()
    {
        popupUI.EnableWinGroup(false);
        popupUI.Hide();
        DataManager.Instance.PlayerData.SetNextLevel();
        GameManager.Instance.StartGame();
    }
    public void OnBackHomeAfterWinClicked()
    {
        DataManager.Instance.PlayerData.SetNextLevel();
    }

    public void OnMMEvent(LoseGame eventType)
    {
        popupUI.EnableLoseGroup(true);
        AudioManager.Instance.PlayBGM(SoundId.Lose);
    }

    public void OnMMEvent(Confirm eventType)
    {
        switch (eventType.tag)
        {
            case "BackToMenu":
                if (eventType.isYes)
                {
                    ShowMainMenu();
                    GameManager.Instance.ReturnToMenu();
                }
                else
                {
                    popupUI.Hide();
                }
                break;
            case "RetryLevel":
                if (eventType.isYes)
                {
                    marketingPanel.Show();
                }
                else
                {
                    ShowMainMenu();
                }
                break;
        }
    }

    public void OnMMEvent(DataChange eventType)
    {
        mainMenu.UpdatePlayerResources();
    }

    public void ShowMarketingPanel()
    {
        marketingPanel.Show();
    }

    public void OnMMEvent(WinGameEvent eventType)
    {
        AudioManager.Instance.PlaySFX(SoundId.LevelClear);
        popupUI.EnableWinGroup(true);
    }

    public void OnMMEvent(UseHammerTool eventType)
    {
        alertUI.Show("Chọn 1 cube để phá hủy!");
    }
}
