using MoreMountains.Tools;
using UnityEngine;

public class GUIManager : MMSingleton<GUIManager>
{
    [SerializeField] private GUIBase mainMenu;
    [SerializeField] private GUIBase inGameUI;
    [SerializeField] private GUIBase popupUI;
    
    void Start()
    {
        if(mainMenu == null) mainMenu = GetComponentInChildren<GUIMenuController>();
        if(inGameUI == null) inGameUI = GetComponentInChildren<UIGameplayController>();
        if(popupUI == null) popupUI = GetComponentInChildren<PopupController>();
        mainMenu.Show();
        inGameUI.Hide();
        popupUI.Hide();
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
        mainMenu.Show();
    }
    public void ShowPopup()
    {
        popupUI.Show();
    }
}
