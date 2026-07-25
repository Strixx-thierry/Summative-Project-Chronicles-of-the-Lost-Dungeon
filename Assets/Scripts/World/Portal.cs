using UnityEngine;

// The exit: dim/locked until the room is cleared, then glows and completes the level on entry
[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    [SerializeField] private Light glow;
    [SerializeField] private Color lockedColor = new Color(0.7f, 0.2f, 0.15f);
    [SerializeField] private Color openColor = new Color(0.35f, 0.8f, 1f);

    private bool active;

    void Awake()
    {
        // Make sure there is a trigger to walk into
        bool hasTrigger = false;
        foreach (var c in GetComponents<Collider>()) if (c.isTrigger) hasTrigger = true;
        if (!hasTrigger)
        {
            var box = gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
            box.center = new Vector3(0, 1.5f, 0);
            box.size = new Vector3(3.5f, 3f, 3f);
        }

        // Make sure there is a glow light
        if (glow == null)
        {
            var go = new GameObject("Glow");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(0, 2f, 0);
            glow = go.AddComponent<Light>();
            glow.type = LightType.Point;
            glow.range = 14f;
            glow.shadows = LightShadows.None;
        }
        glow.enabled = true;
        glow.color = lockedColor;
        glow.intensity = 6f;
    }

    public void Activate()
    {
        active = true;
        if (glow != null)
        {
            glow.color = openColor;
            glow.intensity = 20f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!active) return;
        if (other.GetComponentInParent<PlayerController>() == null) return;
        GameManager.Instance.Win();
    }
}
