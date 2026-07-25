using UnityEngine;

// One simple exit: red while enemies remain, bright blue when cleared, wins on entry
public class ExitPortal : MonoBehaviour
{
    [SerializeField] private Light glow;
    [SerializeField] private Renderer beam;

    static readonly Color Locked = new Color(0.85f, 0.2f, 0.15f);
    static readonly Color Open = new Color(0.3f, 0.8f, 1f);

    private int total;
    private int defeated;
    private bool open;

    void OnEnable() => GameEvents.EnemyDefeated += OnEnemyDefeated;
    void OnDisable() => GameEvents.EnemyDefeated -= OnEnemyDefeated;

    void Start()
    {
        total = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        RunStats.TotalEnemies = total;
        defeated = 0;
        UpdateHud();
        SetOpen(total == 0);
    }

    void OnEnemyDefeated()
    {
        defeated++;
        UpdateHud();
        if (defeated >= total) SetOpen(true);
    }

    void SetOpen(bool value)
    {
        open = value;
        Color c = value ? Open : Locked;
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

    void UpdateHud() =>
        FindFirstObjectByType<HUDController>()?.SetObjective(defeated, total);
}
