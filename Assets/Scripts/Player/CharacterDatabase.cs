using UnityEngine;

// Registry of playable classes (model + ability). Lives in Resources so it loads at runtime.
[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Chronicles/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public string displayName = "Knight";
        public GameObject model;                       // the character fbx
        public RuntimeAnimatorController controller;   // built from this model's own clips
        public AbilityType ability = AbilityType.Sword;
    }

    public Entry[] classes;

    public Entry Get(int index) =>
        (classes != null && classes.Length > 0)
            ? classes[Mathf.Clamp(index, 0, classes.Length - 1)]
            : null;
}
