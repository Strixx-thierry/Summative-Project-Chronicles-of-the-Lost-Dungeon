using UnityEngine;

// A collectible coin; walk into it to add to your gold
public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private int value = 1;
    [SerializeField] private float spinSpeed = 90f;

    void Update() => transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null) Collect(other.gameObject);
    }

    public void Collect(GameObject collector)
    {
        RunStats.Coins += value;
        GameEvents.RaiseItemCollected(value);
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        Destroy(gameObject);
    }
}
