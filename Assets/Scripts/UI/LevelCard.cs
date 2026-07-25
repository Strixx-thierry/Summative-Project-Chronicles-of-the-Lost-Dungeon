using UnityEngine;
using UnityEngine.UI;

// One level card; lock state shown purely with icons (closed lock / open lock / check)
public class LevelCard : MonoBehaviour
{
    public int level = 1;
    [SerializeField] private GameObject lockedIcon;     // closed padlock
    [SerializeField] private GameObject availableIcon;  // open padlock
    [SerializeField] private GameObject completedIcon;  // check badge
    [SerializeField] private GameObject availableFrame; // amber highlight
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup group;

    public enum State { Locked, Available, Completed }

    public void SetState(State state)
    {
        bool locked = state == State.Locked;
        if (lockedIcon != null) lockedIcon.SetActive(locked);
        if (availableIcon != null) availableIcon.SetActive(state == State.Available);
        if (completedIcon != null) completedIcon.SetActive(state == State.Completed);
        if (availableFrame != null) availableFrame.SetActive(state == State.Available);
        if (button != null) button.interactable = !locked;
        if (group != null) group.alpha = locked ? 0.5f : 1f;
    }
}
