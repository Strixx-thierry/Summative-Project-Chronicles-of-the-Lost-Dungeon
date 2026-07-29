// Pure level-progression rules operating on SaveData (no file IO, so it's unit-testable)
public static class Progression
{
    public static bool IsUnlocked(SaveData data, int level) => level <= data.highestUnlocked;

    public static bool IsCompleted(SaveData data, int level) => data.completed.Contains(level);

    public static void Complete(SaveData data, int level, int coins, float time)
    {
        if (!data.completed.Contains(level)) data.completed.Add(level);
        if (level + 1 > data.highestUnlocked) data.highestUnlocked = level + 1;
        data.totalCoins += coins;

        while (data.bestTimes.Count < level) data.bestTimes.Add(0f);
        float prev = data.bestTimes[level - 1];
        if (prev <= 0f || time < prev) data.bestTimes[level - 1] = time;
    }
}
