using UnityEngine;

// Shows the on-screen joystick/buttons only on mobile builds (conditional compilation).
// Tick "Force Show In Editor" to test on a touchscreen PC, then untick.
public class MobileControlsToggle : MonoBehaviour
{
    [SerializeField] private bool forceShowInEditor = false;

    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        gameObject.SetActive(true);
#else
        gameObject.SetActive(forceShowInEditor);
#endif
    }
}
