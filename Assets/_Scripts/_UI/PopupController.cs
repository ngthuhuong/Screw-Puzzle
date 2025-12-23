using UnityEngine;
using UnityEngine.UI;

public class PopupController : GUIBase
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject winGroup;
    [SerializeField] private GameObject loseGroup;
    [SerializeField] private GameObject confirmGroup;

    [SerializeField] private ConfirmController confirmPanel;

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

    public void EnableLoseGroup(bool enable)
    {
        Show();
        UnableAllGroups();
        loseGroup.SetActive(enable);
    }

    public void EnableConfirmGroup(string message, string tag)
    {
        Show();
        UnableAllGroups();
        confirmGroup.SetActive(true);
        confirmPanel.Show(message, tag);
    }
}