using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;

// Read-only REST call: pulls real weapon stats from the free D&D 5e API
public class WeaponInfoService : MonoBehaviour
{
    static WeaponInfoService instance;
    public static WeaponInfoService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("WeaponInfoService").AddComponent<WeaponInfoService>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    const string BaseUrl = "https://www.dnd5eapi.co/api/equipment/";

    [Serializable] class WeaponData { public string name; public string weapon_category; public string weapon_range; public Damage damage; public Prop[] properties; }
    [Serializable] class Damage { public string damage_dice; public DamageType damage_type; }
    [Serializable] class DamageType { public string name; }
    [Serializable] class Prop { public string name; }

    // Short one-line result (used by the pickup toast)
    public void FetchShort(string slug, Action<string> onResult) => StartCoroutine(FetchRoutine(slug, false, onResult));

    // Full multi-line result (used by the inspect panel)
    public void FetchDetailed(string slug, Action<string> onResult) => StartCoroutine(FetchRoutine(slug, true, onResult));

    IEnumerator FetchRoutine(string slug, bool detailed, Action<string> onResult)
    {
        using (var req = UnityWebRequest.Get(BaseUrl + slug))
        {
            req.timeout = 8;
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onResult?.Invoke(null);   // offline / failed — caller handles gracefully
                yield break;
            }

            try
            {
                var d = JsonUtility.FromJson<WeaponData>(req.downloadHandler.text);
                string dice = d.damage != null ? d.damage.damage_dice : "";
                string type = d.damage != null && d.damage.damage_type != null ? d.damage.damage_type.name : "";

                if (!detailed)
                {
                    onResult?.Invoke($"{d.name} ({dice} {type})".Trim());
                    yield break;
                }

                var sb = new StringBuilder();
                sb.AppendLine($"<b>{d.name}</b>");
                if (!string.IsNullOrEmpty(d.weapon_category))
                    sb.AppendLine($"Category: {d.weapon_category} {d.weapon_range}".Trim());
                if (!string.IsNullOrEmpty(dice))
                    sb.AppendLine($"Damage: {dice} {type}".Trim());
                if (d.properties != null && d.properties.Length > 0)
                {
                    var names = new StringBuilder();
                    foreach (var p in d.properties) { if (names.Length > 0) names.Append(", "); names.Append(p.name); }
                    sb.AppendLine($"Properties: {names}");
                }
                sb.AppendLine("\n<size=70%>Data: dnd5eapi.co</size>");
                onResult?.Invoke(sb.ToString().TrimEnd());
            }
            catch
            {
                onResult?.Invoke(null);
            }
        }
    }
}
