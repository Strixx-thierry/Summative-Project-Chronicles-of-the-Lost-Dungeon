using UnityEngine;

// Short fast melee punch, weaker but quicker than the sword (Strategy)
public class FistsAbility : IAbility
{
    public string Name => "Fists";
    public float Cooldown => 0.25f;

    const float Range = 1.6f;
    const float HalfArc = 60f;

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
            target.TakeDamage(DamageCalculator.Compute(Mathf.RoundToInt(ctx.damage * 0.6f), def, 0f));
        }
    }
}
