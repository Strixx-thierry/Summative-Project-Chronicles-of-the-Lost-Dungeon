using UnityEngine;

// Tracks the current level run so result screens and progression can read it
public static class RunStats
{
    public static int LevelNumber = 1;
    public static string LevelScene = "Level1";
    public static string LevelName = "The Awakening Halls";
    public static int EnemiesDefeated;
    public static int TotalEnemies;
    public static int Coins;
    public static float TimeTaken;

    static float startTime;

    public static void BeginLevel(int number, string name)
    {
        LevelNumber = number;
        LevelScene = "Level" + number;
        LevelName = name;
        EnemiesDefeated = 0;
        TotalEnemies = 0;
        Coins = 0;
        TimeTaken = 0;
        startTime = Time.time;
    }

    public static void EndRun() => TimeTaken = Time.time - startTime;

    public static string FormattedTime =>
        $"{(int)TimeTaken / 60:00}:{(int)TimeTaken % 60:00}";
}
