using UnityEngine;

// Shows the special weapon only when slot 2 is equipped; slot 1 (Slash) is empty-handed
public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private AbilityController abilities;
    [SerializeField] private GameObject swordPrefab;   // Spin Slash (Adventurer)
    [SerializeField] private GameObject clubPrefab;     // Super Punch (Brawler)
    [SerializeField] private GameObject gunPrefab;      // Gun (Gunner) - assign when a model exists
    [SerializeField] private string handBoneName = "Wrist.R";
    [SerializeField] private Vector3 posOffset = new Vector3(0f, 0.1f, 0f);
    [SerializeField] private Vector3 eulerOffset = new Vector3(0f, 0f, 90f);
    [SerializeField] private float targetLength = 0.8f;

    private GameObject current;

    void Start()
    {
        if (abilities == null) abilities = GetComponent<AbilityController>();
        if (abilities != null) abilities.OnAbilitiesChanged += Refresh;
        Refresh();
    }

    void OnDestroy()
    {
        if (abilities != null) abilities.OnAbilitiesChanged -= Refresh;
    }

    void Refresh()
    {
        if (abilities == null || abilities.Abilities.Count == 0) return;
        string name = abilities.Abilities[abilities.CurrentIndex].Name;

        GameObject prefab = name.Contains("Spin") ? swordPrefab
                          : name.Contains("Punch") ? clubPrefab
                          : name.Contains("Gun") ? gunPrefab
                          : null;   // basic Slash = empty hands

        if (current != null) Destroy(current);
        if (prefab != null) Attach(prefab);
    }

    void Attach(GameObject prefab)
    {
        var hand = FindBone(handBoneName);
        if (hand == null) { Debug.LogWarning($"WeaponHolder: bone '{handBoneName}' not found."); return; }

        current = Instantiate(prefab, hand);
        current.transform.localEulerAngles = eulerOffset;

        Vector3 ls = hand.lossyScale;
        current.transform.localPosition = new Vector3(
            posOffset.x / Mathf.Max(0.0001f, ls.x),
            posOffset.y / Mathf.Max(0.0001f, ls.y),
            posOffset.z / Mathf.Max(0.0001f, ls.z));
        current.transform.localScale = Vector3.one;

        var rends = current.GetComponentsInChildren<Renderer>();
        if (rends.Length > 0)
        {
            var b = rends[0].bounds;
            foreach (var r in rends) b.Encapsulate(r.bounds);
            float maxDim = Mathf.Max(b.size.x, b.size.y, b.size.z);
            if (maxDim > 0.0001f) current.transform.localScale *= targetLength / maxDim;
        }
    }

    Transform FindBone(string boneName)
    {
        foreach (var t in GetComponentsInChildren<Transform>(true))
            if (t.name == boneName || t.name.EndsWith(boneName)) return t;
        return null;
    }
}
