using UnityEngine;

// Fires a pooled projectile forward (Strategy + Object Pooling)
public class GunAbility : IAbility
{
    public string Name => "Gun";
    public float Cooldown => 0.35f;

    public void Activate(AbilityContext ctx)
    {
        ctx.animator?.SetTrigger("Attack");
        if (ctx.projectilePool == null) return;

        Vector3 dir = ctx.facing != null ? ctx.facing.forward : ctx.owner.forward;
        dir.y = 0; dir.Normalize();

        var projectile = ctx.projectilePool.Get();
        projectile.Fire(ctx.owner.position + Vector3.up * 1.2f + dir * 0.6f, dir, ctx.damage, ctx.projectilePool);
    }
}
