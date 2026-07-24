using UnityEngine;
using UnityEngine.InputSystem;

// Melee swing: left click hits IDamageables in an arc in front of the player
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform facing;      // the Yaw pivot
    [SerializeField] private float range = 2.4f;
    [SerializeField] private float arc = 140f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private int baseDamage = 30;
    [SerializeField] private float critChance = 0.2f;

    private float timer;

    void Awake()
    {
        if (facing == null) facing = transform.Find("Yaw");
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && timer <= 0f)
        {
            timer = cooldown;
            Swing();
        }
    }

    void Swing()
    {
        Vector3 forward = facing != null ? facing.forward : transform.forward;
        forward.y = 0;

        foreach (var col in Physics.OverlapSphere(transform.position, range))
        {
            // Ignore ourselves
            if (col.GetComponentInParent<PlayerController>() != null) continue;

            var target = col.GetComponentInParent<IDamageable>();
            if (target == null || target.IsDead) continue;

            Vector3 to = col.transform.position - transform.position;
            to.y = 0;
            if (Vector3.Angle(forward, to) > arc * 0.5f) continue;

            int defense = col.GetComponentInParent<Enemy>()?.Defense ?? 0;
            target.TakeDamage(DamageCalculator.Compute(baseDamage, defense, critChance));
        }
    }
}
