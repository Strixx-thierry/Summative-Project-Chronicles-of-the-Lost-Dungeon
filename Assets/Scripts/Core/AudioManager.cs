using UnityEngine;

// Global audio: one music channel, one SFX channel (Singleton)
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip uiClick;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        sfxSource = gameObject.AddComponent<AudioSource>();

        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.8f);
    }

    void Start()
    {
        if (menuMusic != null) PlayMusic(menuMusic);
    }

    public float MusicVolume
    {
        get => musicSource.volume;
        set { musicSource.volume = value; PlayerPrefs.SetFloat("MusicVolume", value); }
    }

    public float SfxVolume
    {
        get => sfxSource.volume;
        set { sfxSource.volume = value; PlayerPrefs.SetFloat("SfxVolume", value); }
    }

    // Switches track only if different, so revisiting the menu doesn't restart it
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayClick() => PlaySfx(uiClick);
}
