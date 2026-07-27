using UnityEngine;

// Brawler special: slow, heavy single-target punch in front
public class SuperPunchAbility : IAbility
{
    public string Name => "Super Punch";
    public float Cooldown => 1.1f;

    const float Range = 2f;
    const float HalfArc = 55f;

    public void Activate(AbilityContext ctx)
    {
        ctx.animator?.SetTrigger("Attack");

        Vector3 fwd = ctx.facing != null ? ctx.facing.forward : ctx.owner.forward;
        fwd.y = 0;

        foreach (var col in Physics.OverlapSphere(ctx.owner.position, Range))
        {
            if (col.GetComponentInParent<PlayerController>() != null) continue;
            var target = col.GetComponentInParent<IDamageable>();
            if (target == null || target.IsDead) continue;

            Vector3 to = col.transform.position - ctx.owner.position; to.y = 0;
            if (Vector3.Angle(fwd, to) > HalfArc) continue;

            int def = col.GetComponentInParent<Enemy>()?.Defense ?? 0;
            target.TakeDamage(DamageCalculator.Compute(ctx.damage * 3, def, 0f));   // heavy hit
        }
    }
}
