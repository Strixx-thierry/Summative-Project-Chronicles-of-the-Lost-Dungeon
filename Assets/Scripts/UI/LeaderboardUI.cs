using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

// Fills a list of rows with the top leaderboard times
public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] rows;
    [SerializeField] private TMP_Text statusText;

    public void Show(string highlightName)
    {
        SetStatus("Loading leaderboard...");
        if (!LeaderboardService.Instance.IsConfigured)
        {
            SetStatus("Leaderboard offline");
            ClearRows();
            return;
        }

        LeaderboardService.Instance.GetTop(20, entries =>
        {
            var top = entries.OrderBy(e => e.timeSeconds).Take(rows.Length).ToList();
            if (top.Count == 0) { SetStatus("No times yet - be the first!"); ClearRows(); return; }

            SetStatus("");
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] == null) continue;
                if (i < top.Count)
                {
                    var e = top[i];
                    bool me = e.name == highlightName;
                    rows[i].text = $"{i + 1}. {e.name}".PadRight(22) + RunStats.Format(e.timeSeconds);
                    rows[i].color = me ? new Color(0.91f, 0.64f, 0.24f) : new Color(0.85f, 0.83f, 0.78f);
                }
                else rows[i].text = "";
            }
        });
    }

    void ClearRows()
    {
        foreach (var r in rows) if (r != null) r.text = "";
    }

    void SetStatus(string s)
    {
        if (statusText != null) statusText.text = s;
    }
}
