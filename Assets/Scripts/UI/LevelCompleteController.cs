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

    // Next level does not exist yet, back to menu for now
    public void OnNextLevel() { Click(); SceneLoader.Load(SceneLoader.MainMenu); }
    public void OnLevelSelect() { Click(); SceneLoader.Load(SceneLoader.MainMenu); }

    void Click() { if (AudioManager.Instance != null) AudioManager.Instance.PlayClick(); }
}
