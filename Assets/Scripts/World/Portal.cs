using UnityEngine;

// The exit door: dim and locked until the room is cleared, then glows and completes the level
public class Portal : MonoBehaviour
{
    [SerializeField] private Light glow;
    [SerializeField] private Color lockedColor = new Color(0.7f, 0.2f, 0.15f);
    [SerializeField] private Color openColor = new Color(0.45f, 0.85f, 1f);

    private bool active;

    void Awake()
    {
        if (glow != null)
        {
            glow.enabled = true;
            glow.color = lockedColor;
            glow.intensity = 4f;   // dim red = locked but visible
        }
    }

    public void Activate()
    {
        active = true;
        if (glow != null)
        {
            glow.color = openColor;
            glow.intensity = 16f;  // bright blue = open
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!active) return;
        if (other.GetComponentInParent<PlayerController>() == null) return;
        GameManager.Instance.Win();
    }
}
