using UnityEngine;

// A weapon lying in the room; walk into it to add that weapon to your loadout
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 60f;

    void Update() => transform.Rotate(0, spinSpeed * Time.deltaTime, 0);

    void OnTriggerEnter(Collider other)
    {
        var abilities = other.GetComponentInParent<AbilityController>();
        if (abilities == null) return;

        // Unlock the player's own class special
        var loader = other.GetComponentInParent<PlayerModelLoader>();
        AbilityType special = loader != null ? loader.Ability : AbilityType.Gun;
        abilities.AddAbility(special);
        SaveManager.Instance.UnlockSpecial();   // persist so later levels keep it
        GameEvents.RaiseWeaponCollected();

        // Pull real-world info on this weapon from the REST API (read-only)
        string slug = special switch
        {
            AbilityType.Gun => "light-crossbow",
            AbilityType.SuperPunch => "club",
            AbilityType.SpinSlash => "greatsword",
            _ => "dagger",
        };
        WeaponInfoService.Instance.FetchShort(slug, info =>
            Toast.Instance.Show(info != null ? "Acquired: " + info + "   [I] Inspect" : "New weapon acquired   [I] Inspect"));

        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        Destroy(gameObject);
    }
}
