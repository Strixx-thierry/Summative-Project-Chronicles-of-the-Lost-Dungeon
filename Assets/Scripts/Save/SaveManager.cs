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

    public bool IsUnlocked(int level) => level <= Data.highestUnlocked;
    public bool IsCompleted(int level) => Data.completed.Contains(level);

    public void CompleteLevel(int level, int coins)
    {
        if (!Data.completed.Contains(level)) Data.completed.Add(level);
        if (level + 1 > Data.highestUnlocked) Data.highestUnlocked = level + 1;
        Data.totalCoins += coins;
        Save();
    }
}
