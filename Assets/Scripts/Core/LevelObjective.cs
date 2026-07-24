using UnityEngine;

// Tracks enemy count and lights the portal when the room is cleared
public class LevelObjective : MonoBehaviour
{
    [SerializeField] private Portal portal;

    private int total;
    private int defeated;

    void OnEnable() => GameEvents.EnemyDefeated += OnEnemyDefeated;
    void OnDisable() => GameEvents.EnemyDefeated -= OnEnemyDefeated;

    void Start()
    {
        total = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        RunStats.TotalEnemies = total;
        defeated = 0;
        UpdateHud();
        if (total == 0) portal?.Activate();
    }

    void OnEnemyDefeated()
    {
        defeated++;
        UpdateHud();
        if (defeated >= total) portal?.Activate();
    }

    void UpdateHud() =>
        FindFirstObjectByType<HUDController>()?.SetObjective(defeated, total);
}
