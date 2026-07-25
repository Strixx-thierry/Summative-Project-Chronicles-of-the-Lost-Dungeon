using UnityEngine;

// A weapon lying in the room; walk into it to add that weapon to your loadout
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private AbilityType weapon = AbilityType.Gun;
    [SerializeField] private float spinSpeed = 60f;

    void Update() => transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

    void OnTriggerEnter(Collider other)
    {
        var abilities = other.GetComponentInParent<AbilityController>();
        if (abilities == null) return;
        abilities.AddAbility(weapon);
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        Destroy(gameObject);
    }
}
