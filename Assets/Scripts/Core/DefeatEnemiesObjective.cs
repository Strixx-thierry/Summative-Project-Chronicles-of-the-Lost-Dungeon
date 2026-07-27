using UnityEngine;
using System;

// Objective: clear every enemy in the level
public class DefeatEnemiesObjective : MonoBehaviour, IObjective
{
    private int total;
    private int defeated;

    public string Label => "Defeat all enemies";
    public string Progress => $"{defeated}/{total}";
    public bool IsComplete => defeated >= total;
    public event Action Changed;

    void OnEnable() => GameEvents.EnemyDefeated += OnEnemyDefeated;
    void OnDisable() => GameEvents.EnemyDefeated -= OnEnemyDefeated;

    void Start()
    {
        total = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        RunStats.TotalEnemies = total;
        Changed?.Invoke();
    }

    void OnEnemyDefeated()
    {
        defeated++;
        Changed?.Invoke();
    }
}
