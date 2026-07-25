using UnityEngine;
using UnityEngine.SceneManagement;

// Central place for scene changes
public static class SceneLoader
{
    public const int TotalLevels = 5;

    public const string MainMenu = "MainMenu";
    public const string LevelSelect = "LevelSelect";
    public const string Leaderboard = "Leaderboard";
    public const string Level1 = "Level1";
    public const string GameOver = "GameOver";
    public const string LevelComplete = "LevelComplete";

    public static void Load(string sceneName) => SceneManager.LoadScene(sceneName);

    public static void Reload() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public static string LevelName(int number) => "Level" + number;

    // Loads a level scene if it exists in the build, else returns to the menu
    public static void LoadLevel(int number)
    {
        string scene = LevelName(number);
        if (Application.CanStreamedLevelBeLoaded(scene)) SceneManager.LoadScene(scene);
        else SceneManager.LoadScene(MainMenu);
    }
}
