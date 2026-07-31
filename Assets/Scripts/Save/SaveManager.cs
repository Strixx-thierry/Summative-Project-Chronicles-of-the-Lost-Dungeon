using UnityEngine;
using System.IO;

// Loads and saves progress as JSON (Singleton). Auto-creates itself on first use.
public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("SaveManager").AddComponent<SaveManager>();
                DontDestroyOnLoad(instance.gameObject);
                instance.Load();
            }
            return instance;
        }
    }

    public SaveData Data { get; private set; } = new SaveData();

    private string Path => System.IO.Path.Combine(Application.persistentDataPath, "chronicles_save.json");

    public void Load()
    {
        if (File.Exists(Path))
            Data = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path)) ?? new SaveData();
        else
            Data = new SaveData();
    }

    public void Save() => File.WriteAllText(Path, JsonUtility.ToJson(Data, true));

    public bool IsUnlocked(int level) => Progression.IsUnlocked(Data, level);
    public bool IsCompleted(int level) => Progression.IsCompleted(Data, level);
    public bool HasName => !string.IsNullOrWhiteSpace(Data.playerName);

    public void SetName(string name)
    {
        Data.playerName = name.Trim();
        Save();
    }

    public void SetClass(int index)
    {
        Data.selectedClass = index;
        Data.specialUnlocked = false;   // a fresh character hasn't found their special yet
        Save();
    }

    public void UnlockSpecial()
    {
        Data.specialUnlocked = true;
        Save();
    }

    // Wipe all progress back to a fresh game
    public void ResetProgress()
    {
        Data = new SaveData();
        Save();
    }

    public void CompleteLevel(int level, int coins, float time)
    {
        Progression.Complete(Data, level, coins, time);
        Save();
    }
}
