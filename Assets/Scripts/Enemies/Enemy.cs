using UnityEngine;

// An enemy: reuses Health, reports its death through GameEvents
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int defense = 0;
    [SerializeField] private float deathDelay = 1.8f;   // let the death animation play

    public int Defense => defense;

    private Health health;
    private bool dead;

    void Awake()
    {
        health = GetComponent<Health>();
        health.OnDied += Die;
    }

    void OnDestroy()
    {
        if (health != null) health.OnDied -= Die;
    }

    void Die()
    {
        if (dead) return;
        dead = true;
        RunStats.EnemiesDefeated++;
        GameEvents.RaiseEnemyDefeated();
        foreach (var col in GetComponentsInChildren<Collider>()) col.enabled = false;
        Destroy(gameObject, deathDelay);
    }
}
