using UnityEngine;

// An interchangeable weapon/attack (Strategy pattern)
public interface IAbility
{
    string Name { get; }
    float Cooldown { get; }
    void Activate(AbilityContext ctx);
}

// Everything an ability needs to do its thing, passed in each activation
public class AbilityContext
{
    public Transform owner;
    public Transform facing;
    public Vector3 aimDirection;   // where the camera/crosshair points
    public Animator animator;
    public ObjectPool<Projectile> projectilePool;
    public int damage;
}
