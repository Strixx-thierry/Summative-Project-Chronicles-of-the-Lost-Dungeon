using UnityEngine;
using TMPro;

// Fills the Level Complete screen with run stats
public class LevelCompleteController : MonoBehaviour
{
    [SerializeField] private TMP_Text goldValue;
    [SerializeField] private TMP_Text enemiesValue;
    [SerializeField] private TMP_Text timeValue;

    void Start()
    {
        goldValue.text = RunStats.Coins.ToString("N0");
        enemiesValue.text = RunStats.EnemiesDefeated.ToString();
        timeValue.text = RunStats.FormattedTime;
    }

    // Load the next level, or fall back to the menu if there isn't one
    public void OnNextLevel() { Click(); SceneLoader.LoadLevel(RunStats.LevelNumber + 1); }
    public void OnLevelSelect() { Click(); SceneLoader.Load(SceneLoader.MainMenu); }

    void Click() { if (AudioManager.Instance != null) AudioManager.Instance.PlayClick(); }
}
