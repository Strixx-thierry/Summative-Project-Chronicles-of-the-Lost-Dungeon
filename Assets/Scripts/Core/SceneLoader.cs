using UnityEngine.SceneManagement;

// Central place for scene changes
public static class SceneLoader
{
    public const string MainMenu = "MainMenu";
    public const string Level1 = "Level1";

    public static void Load(string sceneName) => SceneManager.LoadScene(sceneName);

    public static void Reload() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
