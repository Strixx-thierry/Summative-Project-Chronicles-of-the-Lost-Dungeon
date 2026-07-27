using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Self-creating on-screen message strip (used for weapon-info popups)
public class Toast : MonoBehaviour
{
    static Toast instance;
    public static Toast Instance
    {
        get { if (instance == null) instance = Create(); return instance; }
    }

    private TMP_Text text;
    private CanvasGroup group;
    private Coroutine running;

    static Toast Create()
    {
        var go = new GameObject("Toast");
        DontDestroyOnLoad(go);
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        var scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var t = go.AddComponent<Toast>();

        var panel = new GameObject("Panel", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
        panel.transform.SetParent(go.transform, false);
        var rt = (RectTransform)panel.transform;
        rt.anchorMin = new Vector2(0.5f, 0); rt.anchorMax = new Vector2(0.5f, 0); rt.pivot = new Vector2(0.5f, 0);
        rt.anchoredPosition = new Vector2(0, 150); rt.sizeDelta = new Vector2(900, 84);
        panel.GetComponent<Image>().color = new Color(0.1f, 0.11f, 0.13f, 0.92f);
        t.group = panel.GetComponent<CanvasGroup>();
        t.group.alpha = 0;

        var txt = new GameObject("Text", typeof(RectTransform));
        txt.transform.SetParent(panel.transform, false);
        var trt = txt.GetComponent<RectTransform>();
        trt.anchorMin = Vector2.zero; trt.anchorMax = Vector2.one; trt.offsetMin = new Vector2(20, 0); trt.offsetMax = new Vector2(-20, 0);
        t.text = txt.AddComponent<TextMeshProUGUI>();
        t.text.font = TMP_Settings.defaultFontAsset;
        t.text.alignment = TextAlignmentOptions.Center;
        t.text.fontSize = 30;
        t.text.color = new Color(0.91f, 0.9f, 0.86f);

        return t;
    }

    public void Show(string message, float duration = 3f)
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(ShowRoutine(message, duration));
    }

    IEnumerator ShowRoutine(string message, float duration)
    {
        text.text = message;
        group.alpha = 1f;
        yield return new WaitForSeconds(duration);
        float fade = 0.5f, e = 0f;
        while (e < fade) { e += Time.unscaledDeltaTime; group.alpha = 1f - e / fade; yield return null; }
        group.alpha = 0f;
    }
}
