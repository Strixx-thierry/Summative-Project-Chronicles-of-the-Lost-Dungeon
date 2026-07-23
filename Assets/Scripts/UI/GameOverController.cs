using UnityEngine;
using TMPro;

// Fills the You Have Fallen screen with run progress
public class GameOverController : MonoBehaviour
{
    [SerializeField] private TMP_Text objectivesValue;
    [SerializeField] private TMP_Text coinsValue;

    void Start()
    {
        objectivesValue.text = $"{RunStats.EnemiesDefeated} / {RunStats.TotalEnemies}";
        coinsValue.text = RunStats.Coins.ToString("N0");
    }

    public void OnRetry() { Click(); SceneLoader.Load(RunStats.LevelScene); }
    public void OnAbandon() { Click(); SceneLoader.Load(SceneLoader.MainMenu); }

    void Click() { if (AudioManager.Instance != null) AudioManager.Instance.PlayClick(); }
}
