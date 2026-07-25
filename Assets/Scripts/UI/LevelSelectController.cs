using UnityEngine;

// Fills level cards from saved progress and launches the chosen level
public class LevelSelectController : MonoBehaviour
{
    [SerializeField] private LevelCard[] cards;

    // Refresh every time the popup opens so newly unlocked levels update
    void OnEnable() => Refresh();

    void Refresh()
    {
        var save = SaveManager.Instance;
        foreach (var card in cards)
        {
            if (card == null) continue;
            LevelCard.State state = save.IsCompleted(card.level) ? LevelCard.State.Completed
                                  : save.IsUnlocked(card.level) ? LevelCard.State.Available
                                  : LevelCard.State.Locked;
            card.SetState(state);
        }
    }

    // Hooked to each unlocked card's button
    public void OnCardClicked(int level)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        if (SaveManager.Instance.IsUnlocked(level)) SceneLoader.LoadLevel(level);
    }

    public void OnBack()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClick();
        SceneLoader.Load(SceneLoader.MainMenu);
    }
}
