using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

// Shows the equipped weapon on the HUD, and an inspect panel (I) with REST-API weapon stats
public class WeaponInspect : MonoBehaviour
{
    private AbilityController abilities;
    private TMP_Text indicator;
    private GameObject panel;
    private TMP_Text panelText;
    private bool open;

    void Start()
    {
        abilities = GetComponent<AbilityController>();
        BuildUI();
        if (abilities != null) abilities.OnAbilitiesChanged += UpdateIndicator;
        UpdateIndicator();
    }

    void OnDestroy()
    {
        if (abilities != null) abilities.OnAbilitiesChanged -= UpdateIndicator;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
            Toggle();
    }

    void Toggle()
    {
        open = !open;
        panel.SetActive(open);
        if (open) Fetch();
    }

    void Fetch()
    {
        panelText.text = "Loading weapon data...";
        string slug = CurrentSlug();
        WeaponInfoService.Instance.FetchDetailed(slug, info =>
            panelText.text = info != null ? info : "Weapon codex unavailable (offline).");
    }

    string CurrentName() =>
        abilities != null && abilities.Abilities.Count > 0 ? abilities.Abilities[abilities.CurrentIndex].Name : "Slash";

    string CurrentSlug()
    {
        string n = CurrentName();
        if (n.Contains("Spin")) return "greatsword";
        if (n.Contains("Punch")) return "club";
        if (n.Contains("Gun")) return "light-crossbow";
        return "dagger";
    }

    void UpdateIndicator()
    {
        if (indicator != null) indicator.text = $"Weapon: {CurrentName()}   [I] Inspect";
    }

    void BuildUI()
    {
        var canvasGo = new GameObject("WeaponInspectCanvas");
        canvasGo.transform.SetParent(transform);
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 500;
        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGo.AddComponent<GraphicRaycaster>();

        // bottom-centre indicator
        indicator = Text(canvasGo.transform, "Indicator", 26, TextAnchor(0.5f, 0), new Vector2(0, 40), new Vector2(700, 40));
        indicator.alignment = TextAlignmentOptions.Center;

        // inspect panel (hidden)
        panel = new GameObject("InspectPanel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(canvasGo.transform, false);
        var prt = (RectTransform)panel.transform;
        prt.sizeDelta = new Vector2(640, 420);
        prt.anchoredPosition = Vector2.zero;
        var img = panel.GetComponent<Image>();
        img.color = new Color(0.1f, 0.11f, 0.13f, 0.96f);
        var outline = panel.AddComponent<Outline>();
        outline.effectColor = new Color(0.29f, 0.31f, 0.35f);
        outline.effectDistance = new Vector2(3, -3);

        var title = Text(panel.transform, "Title", 34, null, new Vector2(0, 160), new Vector2(580, 50));
        title.text = "WEAPON CODEX";
        title.alignment = TextAlignmentOptions.Center;
        title.fontStyle = FontStyles.Bold;
        title.color = new Color(0.91f, 0.64f, 0.24f);

        panelText = Text(panel.transform, "Body", 26, null, new Vector2(0, -10), new Vector2(560, 260));
        panelText.alignment = TextAlignmentOptions.TopLeft;

        var hint = Text(panel.transform, "Hint", 20, null, new Vector2(0, -180), new Vector2(560, 30));
        hint.text = "[I] to close";
        hint.alignment = TextAlignmentOptions.Center;
        hint.color = new Color(0.5f, 0.53f, 0.57f);

        panel.SetActive(false);
    }

    Vector2 TextAnchor(float x, float y) => new Vector2(x, y);

    TMP_Text Text(Transform parent, string name, float size, Vector2? anchor, Vector2 pos, Vector2 dims)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.font = TMP_Settings.defaultFontAsset;
        tmp.fontSize = size;
        tmp.color = new Color(0.91f, 0.9f, 0.86f);
        var rt = tmp.rectTransform;
        if (anchor.HasValue) { rt.anchorMin = rt.anchorMax = rt.pivot = anchor.Value; }
        rt.anchoredPosition = pos;
        rt.sizeDelta = dims;
        return tmp;
    }
}
