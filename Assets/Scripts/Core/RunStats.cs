using UnityEngine;

// Tracks the current level run so result screens can show progress
public static class RunStats
{
    public static string LevelScene = "Level1";
    public static string LevelName = "The Awakening Halls";
    public static int EnemiesDefeated;
    public static int TotalEnemies;
    public static int Coins;
    public static float TimeTaken;

    static float startTime;

    public static void BeginLevel(string scene, string name, int totalEnemies)
    {
        LevelScene = scene;
        LevelName = name;
        TotalEnemies = totalEnemies;
        EnemiesDefeated = 0;
        Coins = 0;
        TimeTaken = 0;
        startTime = Time.time;
    }

    public static void EndRun() => TimeTaken = Time.time - startTime;

    public static string FormattedTime =>
        $"{(int)TimeTaken / 60:00}:{(int)TimeTaken % 60:00}";
}
