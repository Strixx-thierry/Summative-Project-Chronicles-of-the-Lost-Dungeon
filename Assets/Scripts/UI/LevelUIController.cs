using UnityEngine;
using UnityEngine.InputSystem;

// In-level UI: pause panel and temporary debug buttons
public class LevelUIController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    void Start()
    {
        RunStats.BeginLevel(SceneLoader.Level1, "The Awakening Halls", 4);
        GameManager.Instance.OnStateChanged += HandleState;
        HandleState(GameManager.Instance.State);
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= HandleState;
    }

    // Esc toggles pause
    void Update()
    {
        if (Keyboard.current == null || !Keyboard.current.escapeKey.wasPressedThisFrame) return;
        if (GameManager.Instance.State == GameManager.GameState.Playing) GameManager.Instance.Pause();
        else if (GameManager.Instance.State == GameManager.GameState.Paused) GameManager.Instance.Resume();
    }

    void HandleState(GameManager.GameState state) =>
        pausePanel.SetActive(state == GameManager.GameState.Paused);

    public void OnResume() { Click(); GameManager.Instance.Resume(); }
    public void OnRetry() { Click(); SceneLoader.Reload(); }
    public void OnQuitToMenu() { Click(); SceneLoader.Load(SceneLoader.MainMenu); }

    // Temporary buttons with sample progress until combat exists
    public void OnDebugWin()
    {
        RunStats.EnemiesDefeated = 4;
        RunStats.Coins = 250;
        GameManager.Instance.Win();
    }

    public void OnDebugLose()
    {
        RunStats.EnemiesDefeated = 2;
        RunStats.Coins = 120;
        GameManager.Instance.Lose();
    }

    void Click() { if (AudioManager.Instance != null) AudioManager.Instance.PlayClick(); }
}
