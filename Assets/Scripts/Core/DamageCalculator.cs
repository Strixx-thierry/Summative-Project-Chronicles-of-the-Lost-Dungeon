using UnityEngine;

// Algorithm: final = base * crit * (100 / (100 + defense))
public static class DamageCalculator
{
    public static int Compute(int baseDamage, int defense, float critChance, float critMultiplier = 2f)
    {
        float crit = Random.value < critChance ? critMultiplier : 1f;
        float mitigation = 100f / (100f + Mathf.Max(0, defense));
        return Mathf.Max(1, Mathf.RoundToInt(baseDamage * crit * mitigation));
    }
}
