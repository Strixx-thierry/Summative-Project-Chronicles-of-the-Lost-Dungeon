using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Gameplay HUD: health segments, counters, level info and objective progress
public class HUDController : MonoBehaviour
{
    [SerializeField] private Transform healthSegments;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text keysText;
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private TMP_Text objectiveCountText;
    [SerializeField] private Image objectiveFill;

    static readonly Color SegmentOn = new Color(0.70f, 0.23f, 0.23f);
    static readonly Color SegmentOff = new Color(0.10f, 0.11f, 0.13f);

    void Start()
    {
        var player = FindFirstObjectByType<PlayerController>();
        var health = player != null ? player.GetComponent<Health>() : null;
        if (health != null)
        {
            health.OnChanged += SetHealth;
            SetHealth(health.Current, health.Max);
        }
        SetCoins(0);
        SetKeys(0);
        levelNameText.text = "Level 1 — " + RunStats.LevelName;
        SetObjective(RunStats.EnemiesDefeated, RunStats.TotalEnemies);
    }

    public void SetHealth(int current, int max)
    {
        healthText.text = $"{current} / {max}";
        int count = healthSegments.childCount;
        int lit = Mathf.CeilToInt((float)current / max * count);
        for (int i = 0; i < count; i++)
            healthSegments.GetChild(i).GetComponent<Image>().color = i < lit ? SegmentOn : SegmentOff;
    }

    public void SetCoins(int amount) => coinsText.text = amount.ToString("N0");

    public void SetKeys(int amount) => keysText.text = amount.ToString();

    public void SetObjective(int done, int total)
    {
        objectiveCountText.text = $"{done}/{total}";
        objectiveFill.fillAmount = total > 0 ? (float)done / total : 0f;
    }
}
