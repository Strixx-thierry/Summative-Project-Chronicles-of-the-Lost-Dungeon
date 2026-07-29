using UnityEngine;
using System;

// Objective: pick up every coin in the level
public class CollectCoinsObjective : MonoBehaviour, IObjective
{
    private int total;
    private int collected;

    public string Label => "Collect the coins";
    public string Progress => $"{collected}/{total}";
    public bool IsComplete => collected >= total;
    public event Action Changed;

    void OnEnable() => GameEvents.ItemCollected += OnItemCollected;
    void OnDisable() => GameEvents.ItemCollected -= OnItemCollected;

    void Start()
    {
        total = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;
        Changed?.Invoke();
    }

    void OnItemCollected(int value)
    {
        collected += value;
        Changed?.Invoke();
    }
}
