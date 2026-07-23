using UnityEngine;
using System;

// Hit points with events so UI and game flow react without direct references
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 200;

    public int Current { get; private set; }
    public int Max => maxHealth;

    public event Action<int, int> OnChanged;
    public event Action OnDied;

    void Awake() => Current = maxHealth;

    public void TakeDamage(int amount)
    {
        if (Current <= 0) return;
        Current = Mathf.Max(0, Current - amount);
        OnChanged?.Invoke(Current, maxHealth);
        if (Current == 0) OnDied?.Invoke();
    }

    public void Heal(int amount)
    {
        if (Current <= 0) return;
        Current = Mathf.Min(maxHealth, Current + amount);
        OnChanged?.Invoke(Current, maxHealth);
    }
}
