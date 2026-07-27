using UnityEngine;

// The exit: red while locked, blue once opened by the ObjectiveManager, wins on entry
public class ExitPortal : MonoBehaviour
{
    [SerializeField] private Light glow;
    [SerializeField] private Renderer beam;

    static readonly Color Locked = new Color(0.85f, 0.2f, 0.15f);
    static readonly Color OpenColor = new Color(0.3f, 0.8f, 1f);

    private bool open;

    void Start() => SetVisual(false);

    public void Open()
    {
        if (open) return;
        open = true;
        SetVisual(true);
    }

    void SetVisual(bool value)
    {
        Color c = value ? OpenColor : Locked;
        if (glow != null)
        {
            glow.color = c;
            glow.intensity = value ? 24f : 4f;
        }
        if (beam != null)
        {
            beam.material.color = c;
            beam.material.EnableKeyword("_EMISSION");
            beam.material.SetColor("_EmissionColor", c * (value ? 4f : 1.5f));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!open) return;
        if (other.GetComponentInParent<PlayerController>() == null) return;
        GameManager.Instance.Win();
    }
}
