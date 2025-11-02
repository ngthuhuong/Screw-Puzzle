using System;
using MoreMountains.Tools;
using UnityEngine;

public class GUIManager : MMSingleton<GUIManager>,MMEventListener<LoseGame>,MMEventListener<Confirm>
{
    [SerializeField] private GUIMenuController mainMenu;
    [SerializeField] private UIGameplayController inGameUI;
    [SerializeField] private PopupController popupUI;
    [SerializeField] private MarketingPanel marketingPanel;
    
    void Start()
    {
        if(mainMenu == null) mainMenu = GetComponentInChildren<GUIMenuController>();
        if(inGameUI == null) inGameUI = GetComponentInChildren<UIGameplayController>();
        if(popupUI == null) popupUI = GetComponentInChildren<PopupController>();
        if(marketingPanel == null) marketingPanel = GetComponentInChildren<MarketingPanel>();
       // mainMenu.Show();
      //  inGameUI.Hide();
      //
        mainMenu.Hide();
        inGameUI.Show();
        popupUI.Hide();
        marketingPanel.Hide();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<LoseGame>();
        this.MMEventStartListening<Confirm>();

    }
    private void OnDisable()
    {
        this.MMEventStopListening<LoseGame>();
        this.MMEventStopListening<Confirm>();
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
    public void OnMMEvent(LoseGame eventType)
    {
        popupUI.EnableLoseGroup(true);
    }

    public void OnMMEvent(Confirm eventType)
    {
        switch (eventType.tag)
        {
            case "BackToMenu":
                if (eventType.isYes)
                {
                    ShowMainMenu();
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
}
