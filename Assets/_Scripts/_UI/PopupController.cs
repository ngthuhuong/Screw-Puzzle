using UnityEngine;

public class PopupController : GUIBase
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject winGroup;
    [SerializeField] private GameObject loseGroup;
    [SerializeField] private GameObject confirmGroup;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void EnableConfirmGroup(bool enable)
    {
        Show();
        UnableAllGroups();
        confirmGroup.SetActive(enable);
    }
    public void EnableLoseGroup(bool enable)
    {
        Show();
        UnableAllGroups();
        loseGroup.SetActive(enable);
    }
}
