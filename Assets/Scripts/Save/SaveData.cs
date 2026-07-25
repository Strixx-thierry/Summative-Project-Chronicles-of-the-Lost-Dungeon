using System;
using System.Collections.Generic;

// Serializable snapshot of persistent progress
[Serializable]
public class SaveData
{
    public string playerName = "";
    public int highestUnlocked = 1;
    public List<int> completed = new List<int>();
    public List<float> bestTimes = new List<float>();   // index = level-1
    public int totalCoins = 0;
    public float musicVolume = 0.8f;
    public float sfxVolume = 0.8f;
}
