using UnityEngine;

// A healing item; walk into it to restore health
public class HealthPickup : MonoBehaviour, ICollectable
{
    [SerializeField] private int healAmount = 60;
    [SerializeField] private float spinSpeed = 60f;

    void Update() => transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null) Collect(other.gameObject);
    }

    public void Collect(GameObject collector)
    {
        var health = collector.GetComponentInParent<PlayerController>()?.GetComponent<Health>();
        if (health == null) return;
        if (health.Current >= health.Max) return;   // no waste at full HP

        health.Heal(healAmount);
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        Destroy(gameObject);
    }
}
