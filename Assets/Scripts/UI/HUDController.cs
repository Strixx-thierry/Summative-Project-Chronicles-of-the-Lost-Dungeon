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

    private TMP_Text timerText;

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
        SetCoins(RunStats.Coins);
        SetKeys(0);
        levelNameText.text = "Level " + RunStats.LevelNumber + " — " + RunStats.LevelName;
        SetObjective(RunStats.EnemiesDefeated, RunStats.TotalEnemies);
        CreateTimer();
        GameEvents.ItemCollected += OnItemCollected;
    }

    void OnDestroy() => GameEvents.ItemCollected -= OnItemCollected;

    void OnItemCollected(int value) => SetCoins(RunStats.Coins);

    void Update()
    {
        if (timerText != null) timerText.text = RunStats.FormattedElapsed;
    }

    // Builds a top-centre timer at runtime so every level scene gets one
    void CreateTimer()
    {
        var go = new GameObject("Timer", typeof(RectTransform));
        go.transform.SetParent(transform, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0, -24);
        rt.sizeDelta = new Vector2(240, 60);

        timerText = go.AddComponent<TextMeshProUGUI>();
        timerText.font = TMP_Settings.defaultFontAsset;
        timerText.fontSize = 40;
        timerText.color = new Color(0.91f, 0.90f, 0.86f);
        timerText.alignment = TextAlignmentOptions.Center;
        timerText.fontStyle = FontStyles.Bold;
        timerText.text = "00:00";
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
