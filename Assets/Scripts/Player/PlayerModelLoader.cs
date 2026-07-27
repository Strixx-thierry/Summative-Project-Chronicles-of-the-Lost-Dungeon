using UnityEngine;

// Swaps in the model for the player's chosen class before other scripts read the animator
public class PlayerModelLoader : MonoBehaviour
{
    [SerializeField] private Transform yaw;
    [SerializeField] private RuntimeAnimatorController controller;

    public AbilityType Ability { get; private set; } = AbilityType.SpinSlash;

    void Awake()
    {
        var db = Resources.Load<CharacterDatabase>("CharacterDatabase");
        if (db == null) return;

        var entry = db.Get(SaveManager.Instance.Data.selectedClass);
        if (entry == null || entry.model == null) return;
        Ability = entry.ability;

        if (yaw == null) yaw = transform.Find("Yaw");
        if (yaw == null) return;

        // Keep whatever facing the prefab's model used (same pack, same orientation)
        var existing = yaw.Find("Model");
        Quaternion modelRot = existing != null ? existing.localRotation : Quaternion.Euler(0, 180f, 0);
        if (existing != null)
        {
            // Deactivate now so GetComponentInChildren<Animator> finds only the new model
            existing.gameObject.SetActive(false);
            Destroy(existing.gameObject);
        }

        var model = Instantiate(entry.model, yaw);
        model.name = "Model";
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = modelRot;

        // Normalise height to ~1.9 units
        float h = MeasureHeight(model);
        if (h > 0.1f && (h < 1.4f || h > 2.6f)) model.transform.localScale *= 1.9f / h;

        var animator = model.GetComponent<Animator>();
        if (animator == null) animator = model.AddComponent<Animator>();
        // Prefer this class's own controller (its own clips), fall back to the shared one
        animator.runtimeAnimatorController = entry.controller != null ? entry.controller : controller;
        animator.applyRootMotion = false;
    }

    static float MeasureHeight(GameObject go)
    {
        var rends = go.GetComponentsInChildren<Renderer>();
        if (rends.Length == 0) return 0f;
        var b = rends[0].bounds;
        foreach (var r in rends) b.Encapsulate(r.bounds);
        return b.size.y;
    }
}
