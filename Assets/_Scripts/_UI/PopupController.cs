using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : GUIBase
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject winGroup;
    [SerializeField] private GameObject loseGroup;
    [SerializeField] private GameObject confirmGroup;
    [SerializeField] public Button backToMenuButton;
    
    [SerializeField] private ConfirmController confirmPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backToMenuButton.onClick.AddListener(HandleBackToMenuButton);
      
    }


    private void UnableAllGroups()
    {
        buttonGroup.SetActive(false);
        winGroup.SetActive(false);
        loseGroup.SetActive(false);
        confirmGroup.SetActive(false);
    }
    public void EnableButtonGroup(bool enable)
    {
        Show();
        UnableAllGroups();
        buttonGroup.SetActive(enable);
    }

    public void EnableWinGroup(bool enable)
    {
        Show();
        UnableAllGroups();
        winGroup.SetActive(enable);
    }
    public void EnableConfirmGroup(string message, string tag)
    {
        Show();
        UnableAllGroups();
        confirmGroup.SetActive(true);
        confirmPanel.Show(message, tag);
    }
    public void EnableLoseGroup(bool enable)
    {
        Show();
        UnableAllGroups();
        loseGroup.SetActive(enable);
    }
        
    private void HandleBackToMenuButton()
    {
        EnableConfirmGroup("Are you sure you want to return to the main menu?","BackToMenu");
    }
    
    public void HandleLoseGame()
    {
        EnableLoseGroup(true);
    }
}
