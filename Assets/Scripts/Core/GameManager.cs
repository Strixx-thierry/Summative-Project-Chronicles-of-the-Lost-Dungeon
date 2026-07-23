using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// Game flow state machine (Singleton): Playing, Paused, Won, Lost
public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Paused, Won, Lost }

    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; } = GameState.Playing;

    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (_, _) => SetState(GameState.Playing);
    }

    // Win saves completion, lose does not - progress is only kept after finishing
    public void Win()
    {
        SetState(GameState.Won);
        RunStats.EndRun();
        PlayerPrefs.SetInt(RunStats.LevelScene + "_Completed", 1); // temp save until JSON system
        SceneLoader.Load(SceneLoader.LevelComplete);
    }

    public void Lose()
    {
        SetState(GameState.Lost);
        RunStats.EndRun();
        SceneLoader.Load(SceneLoader.GameOver);
    }
    public void Pause() { if (State == GameState.Playing) SetState(GameState.Paused); }
    public void Resume() { if (State == GameState.Paused) SetState(GameState.Playing); }

    // Freezes time whenever gameplay is not running
    void SetState(GameState next)
    {
        State = next;
        Time.timeScale = next == GameState.Playing ? 1f : 0f;
        OnStateChanged?.Invoke(next);
    }
}
