using UnityEngine;

// Submits the player's total time then shows the leaderboard; has a menu button
public class LeaderboardSceneController : MonoBehaviour
{
    [SerializeField] private LeaderboardUI leaderboard;

    void Start()
    {
        var save = SaveManager.Instance;
        string name = string.IsNullOrWhiteSpace(save.Data.playerName) ? "Adventurer" : save.Data.playerName;

        float total = 0f;
        foreach (var t in save.Data.bestTimes) total += t;

        // Submit first, then refresh so the player's own time is included
        if (total > 0f)
            LeaderboardService.Instance.Submit(name, total, () => leaderboard.Show(name));
        else
            leaderboard.Show(name);
    }

    public void OnMainMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        SceneLoader.Load(SceneLoader.MainMenu);
    }
}
