using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

// Simple time-based online leaderboard over the dreamlo REST API (Singleton)
public class LeaderboardService : MonoBehaviour
{
    // Create a free board at https://dreamlo.com/ and paste your codes here
    const string PrivateCode = "REPLACE_WITH_PRIVATE_CODE";
    const string PublicCode = "REPLACE_WITH_PUBLIC_CODE";
    const string BaseUrl = "https://dreamlo.com/lb/";

    // Faster time = higher stored score, so dreamlo's descending sort shows the best first
    const int ScoreBase = 100000000;

    private static LeaderboardService instance;
    public static LeaderboardService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("LeaderboardService").AddComponent<LeaderboardService>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    public bool IsConfigured =>
        PublicCode != "REPLACE_WITH_PUBLIC_CODE" && PrivateCode != "REPLACE_WITH_PRIVATE_CODE";

    public static int TimeToScore(float seconds) => Mathf.Max(0, ScoreBase - Mathf.RoundToInt(seconds * 100f));
    public static float ScoreToTime(int score) => (ScoreBase - score) / 100f;

    public void Submit(string playerName, float timeSeconds, Action onDone = null)
    {
        if (!IsConfigured) { onDone?.Invoke(); return; }
        StartCoroutine(SubmitRoutine(playerName, TimeToScore(timeSeconds), onDone));
    }

    public void GetTop(int count, Action<List<LeaderboardEntry>> onResult)
    {
        if (!IsConfigured) { onResult?.Invoke(new List<LeaderboardEntry>()); return; }
        StartCoroutine(GetRoutine(count, onResult));
    }

    IEnumerator SubmitRoutine(string name, int score, Action onDone)
    {
        string url = BaseUrl + PrivateCode + "/add/" + UnityWebRequest.EscapeURL(name) + "/" + score;
        using (var req = UnityWebRequest.Get(url))
        {
            req.timeout = 8;
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
                Debug.LogWarning("Leaderboard submit failed: " + req.error);
            onDone?.Invoke();
        }
    }

    IEnumerator GetRoutine(int count, Action<List<LeaderboardEntry>> onResult)
    {
        // pipe format is easy to parse: name|score|seconds|text|date per line
        string url = BaseUrl + PublicCode + "/pipe/" + count;
        var result = new List<LeaderboardEntry>();
        using (var req = UnityWebRequest.Get(url))
        {
            req.timeout = 8;
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
            {
                foreach (var line in req.downloadHandler.text.Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split('|');
                    if (parts.Length < 2) continue;
                    if (int.TryParse(parts[1], out int score))
                        result.Add(new LeaderboardEntry { name = parts[0], timeSeconds = ScoreToTime(score) });
                }
            }
            else Debug.LogWarning("Leaderboard fetch failed: " + req.error);
        }
        onResult?.Invoke(result);
    }
}
