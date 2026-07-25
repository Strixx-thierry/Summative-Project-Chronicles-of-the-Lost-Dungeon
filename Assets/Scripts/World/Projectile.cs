using UnityEngine;

// A pooled projectile that flies forward and damages the first enemy it hits
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 18f;
    [SerializeField] private float lifetime = 3f;

    private Vector3 direction;
    private int damage;
    private float timer;
    private ObjectPool<Projectile> pool;

    public void Fire(Vector3 position, Vector3 dir, int dmg, ObjectPool<Projectile> owningPool)
    {
        transform.position = position;
        direction = dir.normalized;
        damage = dmg;
        pool = owningPool;
        timer = lifetime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer <= 0f) Despawn();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null) return;
        var target = other.GetComponentInParent<IDamageable>();
        if (target != null && !target.IsDead)
        {
            int def = other.GetComponentInParent<Enemy>()?.Defense ?? 0;
            target.TakeDamage(DamageCalculator.Compute(damage, def, 0f));
            Despawn();
        }
    }

    void Despawn()
    {
        if (pool != null) pool.Return(this);
        else gameObject.SetActive(false);
    }
}
