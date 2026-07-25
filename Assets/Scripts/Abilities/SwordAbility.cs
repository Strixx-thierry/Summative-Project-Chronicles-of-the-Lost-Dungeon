using UnityEngine;

// Melee arc in front of the player (Strategy)
public class SwordAbility : IAbility
{
    public string Name => "Sword";
    public float Cooldown => 0.5f;

    const float Range = 2.4f;
    const float HalfArc = 70f;

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
            target.TakeDamage(DamageCalculator.Compute(ctx.damage, def, 0f));
        }
    }
}
