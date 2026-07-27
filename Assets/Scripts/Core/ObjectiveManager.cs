using UnityEngine;
using TMPro;
using System.Text;

// Tracks all objectives in the level, lists them on the HUD, opens the exit when all are done
public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private ExitPortal exit;
    [SerializeField] private TMP_Text objectiveText;

    private IObjective[] objectives;

    void Start()
    {
        objectives = GetComponents<IObjective>();
        foreach (var o in objectives) o.Changed += Refresh;
        if (exit == null) exit = FindFirstObjectByType<ExitPortal>();
        Refresh();
    }

    void OnDestroy()
    {
        if (objectives == null) return;
        foreach (var o in objectives) o.Changed -= Refresh;
    }

    void Refresh()
    {
        bool all = true;
        var sb = new StringBuilder();
        foreach (var o in objectives)
        {
            string tick = o.IsComplete ? "<color=#5E8C4A>V</color> " : "";
            sb.AppendLine($"{tick}{o.Label}   {o.Progress}");
            if (!o.IsComplete) all = false;
        }

        if (objectiveText != null) objectiveText.text = sb.ToString().TrimEnd();
        if (all && objectives.Length > 0) exit?.Open();
    }
}
