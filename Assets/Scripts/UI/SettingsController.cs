using UnityEngine;
using UnityEngine.UI;

// Settings sliders -> AudioManager volumes
public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // Panel starts inactive, so sync slider positions every time it opens
    void OnEnable()
    {
        if (AudioManager.Instance == null) return;
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.MusicVolume);
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.SfxVolume);
    }

    public void OnMusicChanged(float value) => AudioManager.Instance.MusicVolume = value;

    public void OnSfxChanged(float value) => AudioManager.Instance.SfxVolume = value;
}
