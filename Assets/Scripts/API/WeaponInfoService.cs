using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

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

    [Serializable] class WeaponData { public string name; public Damage damage; }
    [Serializable] class Damage { public string damage_dice; public DamageType damage_type; }
    [Serializable] class DamageType { public string name; }

    public void Fetch(string slug, Action<string> onResult) => StartCoroutine(FetchRoutine(slug, onResult));

    IEnumerator FetchRoutine(string slug, Action<string> onResult)
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
                onResult?.Invoke($"{d.name}  ({dice} {type})".Trim());
            }
            catch
            {
                onResult?.Invoke(null);
            }
        }
    }
}
