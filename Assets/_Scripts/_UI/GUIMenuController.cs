using UnityEngine;
using UnityEngine.UI;

public class GUIMenuController : GUIBase
{
    [SerializeField]
    public Button playButton;
    public Button playerProfileButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(HandlePlayButton);
    }


    private void HandlePlayButton()
    {
        GameManager.Instance.ResumeGame();
    }
}
