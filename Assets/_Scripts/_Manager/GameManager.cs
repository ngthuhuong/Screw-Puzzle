using UnityEngine;
using MoreMountains.Tools;

public enum GameState { MainMenu, Playing, Paused, GameOver }

public class GameManager : MMSingleton<GameManager>
{
    // Giữ nguyên phần của bạn
    public bool InputLocked { get; private set; }

    // Trạng thái hiện tại của game
    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    [Header("Scene References")]
    public GameObject mainMenu;      // UI Trang chủ
    public GameObject gameplayUI;    // UI Khi chơi
  //  public GameObject pauseMenu;     // UI Tạm dừng
    //public GameObject gameOverUI;    // UI Kết thúc
    public GameObject gameplayRoot;  // Toàn bộ object gameplay

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

        // Ẩn/hiện các UI tương ứng
        mainMenu.SetActive(newState == GameState.MainMenu);
        gameplayUI.SetActive(newState == GameState.Playing);
       // pauseMenu.SetActive(newState == GameState.Paused);
       //gameOverUI.SetActive(newState == GameState.GameOver);

        // Bật / tắt gameplay root
        gameplayRoot.SetActive(newState == GameState.Playing || newState == GameState.Paused);

        // Dừng thời gian nếu cần
        Time.timeScale = (newState == GameState.Paused || newState == GameState.GameOver) ? 0 : 1;

        // Khóa input trong các trạng thái không phải Playing
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
