using UnityEngine;

// Knight special: hits everything around the player (360, no arc limit)
public class SpinSlashAbility : IAbility
{
    public string Name => "Spin Slash";
    public float Cooldown => 0.9f;

    const float Range = 3f;

    public void Activate(AbilityContext ctx)
    {
        ctx.animator?.SetTrigger("Attack");

        foreach (var col in Physics.OverlapSphere(ctx.owner.position, Range))
        {
            if (col.GetComponentInParent<PlayerController>() != null) continue;
            var target = col.GetComponentInParent<IDamageable>();
            if (target == null || target.IsDead) continue;

            int def = col.GetComponentInParent<Enemy>()?.Defense ?? 0;
            target.TakeDamage(DamageCalculator.Compute(ctx.damage, def, 0f));
        }
    }
}
