using UnityEngine;
using TMPro;

// Fills the Level Complete screen; the final level routes to the leaderboard scene
public class LevelCompleteController : MonoBehaviour
{
    [SerializeField] private TMP_Text goldValue;
    [SerializeField] private TMP_Text enemiesValue;
    [SerializeField] private TMP_Text timeValue;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text nextButtonLabel;

    private bool isFinalLevel;

    void Start()
    {
        goldValue.text = RunStats.Coins.ToString("N0");
        enemiesValue.text = RunStats.EnemiesDefeated.ToString();
        timeValue.text = RunStats.FormattedTime;

        isFinalLevel = RunStats.LevelNumber >= SceneLoader.TotalLevels;
        if (isFinalLevel)
        {
            if (titleText != null) titleText.text = "DUNGEON CONQUERED";
            if (nextButtonLabel != null) nextButtonLabel.text = "MAIN MENU";
        }
    }

    public void OnNextLevel()
    {
        Click();
        if (isFinalLevel) SceneLoader.Load(SceneLoader.MainMenu);
        else SceneLoader.LoadLevel(RunStats.LevelNumber + 1);
    }

    public void OnLevelSelect() { Click(); SceneLoader.Load(SceneLoader.MainMenu); }

    void Click() { if (AudioManager.Instance != null) AudioManager.Instance.PlayClick(); }
}
