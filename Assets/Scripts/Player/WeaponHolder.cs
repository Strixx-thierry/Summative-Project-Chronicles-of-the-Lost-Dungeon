using UnityEngine;

// Shows the class's weapon in hand: Knight = sword, Brawler = club, Gunner = none
public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private GameObject clubPrefab;
    [SerializeField] private string handBoneName = "Wrist.R";
    [SerializeField] private Vector3 posOffset = new Vector3(0f, 0.1f, 0f);
    [SerializeField] private Vector3 eulerOffset = new Vector3(0f, 0f, 90f);
    [SerializeField] private float targetLength = 0.8f;

    void Start()
    {
        var loader = GetComponent<PlayerModelLoader>();
        AbilityType cls = loader != null ? loader.Ability : AbilityType.SpinSlash;

        GameObject prefab = cls == AbilityType.SuperPunch ? clubPrefab   // Brawler
                          : cls == AbilityType.Gun ? null                 // Gunner: no held weapon
                          : swordPrefab;                                    // Knight
        if (prefab != null) Attach(prefab);
    }

    void Attach(GameObject prefab)
    {
        var hand = FindBone(handBoneName);
        if (hand == null) { Debug.LogWarning($"WeaponHolder: bone '{handBoneName}' not found."); return; }

        var weapon = Instantiate(prefab, hand);
        weapon.transform.localEulerAngles = eulerOffset;

        Vector3 ls = hand.lossyScale;
        weapon.transform.localPosition = new Vector3(
            posOffset.x / Mathf.Max(0.0001f, ls.x),
            posOffset.y / Mathf.Max(0.0001f, ls.y),
            posOffset.z / Mathf.Max(0.0001f, ls.z));
        weapon.transform.localScale = Vector3.one;

        var rends = weapon.GetComponentsInChildren<Renderer>();
        if (rends.Length > 0)
        {
            var b = rends[0].bounds;
            foreach (var r in rends) b.Encapsulate(r.bounds);
            float maxDim = Mathf.Max(b.size.x, b.size.y, b.size.z);
            if (maxDim > 0.0001f) weapon.transform.localScale *= targetLength / maxDim;
        }
    }

    Transform FindBone(string boneName)
    {
        foreach (var t in GetComponentsInChildren<Transform>(true))
            if (t.name == boneName || t.name.EndsWith(boneName)) return t;
        return null;
    }
}
