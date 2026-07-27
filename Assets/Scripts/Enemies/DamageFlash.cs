using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Briefly flashes the model red when its Health takes damage
[RequireComponent(typeof(Health))]
public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = new Color(1f, 0.2f, 0.15f);
    [SerializeField] private float duration = 0.12f;

    private readonly List<Renderer> renderers = new List<Renderer>();
    private readonly List<Color> originals = new List<Color>();
    private Coroutine running;

    void Awake()
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            if (r.material == null || !r.material.HasProperty("_BaseColor")) continue;
            renderers.Add(r);
            originals.Add(r.material.GetColor("_BaseColor"));
        }
        GetComponent<Health>().OnDamaged += Flash;
    }

    void OnDestroy()
    {
        var h = GetComponent<Health>();
        if (h != null) h.OnDamaged -= Flash;
    }

    void Flash()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i < renderers.Count; i++)
            renderers[i].material.SetColor("_BaseColor", flashColor);
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < renderers.Count; i++)
            renderers[i].material.SetColor("_BaseColor", originals[i]);
        running = null;
    }
}
