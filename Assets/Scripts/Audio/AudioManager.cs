using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [Header("SFX")]
    [SerializeField] private Sound[] sounds;

    [Header("Music")]
    [SerializeField] private AudioClip musicClip;
    [Range(0f, 1f)] [SerializeField] private float musicVolume = 0.5f;

    private AudioSource sfxSource;
    private AudioSource musicSource;
    private Dictionary<string, Sound> soundDict = new Dictionary<string, Sound>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        foreach (Sound s in sounds)
            soundDict[s.name] = s;

        // Load saved volume values, fall back to Inspector defaults if not set
        float savedMusic = PlayerPrefs.GetFloat("musicVolume", musicVolume);
        float savedSFX = PlayerPrefs.GetFloat("sfxVolume", 1f);
        sfxSource.volume = savedSFX;

        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.volume = savedMusic;
            musicSource.Play();
        }
    }

    public void Play(string name)
    {
        if (!soundDict.ContainsKey(name))
        {
            Debug.LogWarning($"AudioManager: '{name}' sesi bulunamadi!");
            return;
        }

        Sound s = soundDict[name];
        sfxSource.PlayOneShot(s.clip, s.volume);
    }

    public bool IsReady => musicSource != null && sfxSource != null;
    public float MusicVolume => musicSource != null ? musicSource.volume : 0.5f;
    public float SFXVolume => sfxSource != null ? sfxSource.volume : 1f;

    public void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
    }
}
