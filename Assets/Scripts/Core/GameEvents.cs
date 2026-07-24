using System;

// Central event hub (Observer) so systems talk without direct references
public static class GameEvents
{
    public static event Action EnemyDefeated;
    public static event Action<int> ItemCollected;

    public static void RaiseEnemyDefeated() => EnemyDefeated?.Invoke();
    public static void RaiseItemCollected(int value) => ItemCollected?.Invoke(value);
}
