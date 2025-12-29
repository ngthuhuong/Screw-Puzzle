using UnityEngine;
using MoreMountains.Tools;

public enum GameState { MainMenu, Playing, Paused, GameOver }

public class GameManager : MMSingleton<GameManager>
{
    public bool InputLocked { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.MainMenu;
    [SerializeField] private GameObject gameplayRoot;
    [SerializeField] public ScrewColorPresets palletColor;
    public void LockInput()
    {
        InputLocked = true;
    }

    public void UnlockInput()
    {
        InputLocked = false;
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
    }

    public void PauseGame()
    {
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        SetState(GameState.Playing);
    }

    public void EndGame()
    {
        SetState(GameState.GameOver);
    }

    public void ReturnToMenu()
    {
        SetState(GameState.MainMenu);
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                GUIManager.Instance.ShowMainMenu();
                gameplayRoot.SetActive(false);
                break;
            case GameState.Playing:
                GUIManager.Instance.ShowInGameUI();
                LevelManager.Instance.StartLevel(DataManager.Instance.PlayerData.CurrentLevelIndex);
                gameplayRoot.SetActive(true);
                UnlockInput();
                DataManager.Instance.PlayerData.LoseLife();
                MMEventManager.TriggerEvent(new StartGame());
                break;
            //con win +lose
            
        }
        

        //tạm dưngf + thua
        Time.timeScale = (newState == GameState.Paused || newState == GameState.GameOver) ? 0 : 1;

        if (newState == GameState.Playing)
            UnlockInput();
        else
            LockInput();
    }
    
    protected override void Awake()
    {
        base.Awake(); // rất quan trọng, để MMSingleton hoạt động
    }
}
