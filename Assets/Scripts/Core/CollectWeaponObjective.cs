using UnityEngine;
using System;

// Objective: find and pick up the weapon in the room
public class CollectWeaponObjective : MonoBehaviour, IObjective
{
    private bool collected;

    public string Label => "Find the weapon";
    public string Progress => collected ? "1/1" : "0/1";
    public bool IsComplete => collected;
    public event Action Changed;

    void OnEnable() => GameEvents.WeaponCollected += OnCollected;
    void OnDisable() => GameEvents.WeaponCollected -= OnCollected;

    void OnCollected()
    {
        collected = true;
        Changed?.Invoke();
    }
}
