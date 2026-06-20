using CodeStage.AdvancedFPSCounter;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Optimization Settings Asset")]
    [SerializeField] private OptimizationSettings optimizationSettings;

    [Header("Optimization Toggles")]
    [SerializeField] private Toggle toggleObjectPooling;
    [SerializeField] private Toggle toggleSpatialPartitioning;
    [SerializeField] private Toggle toggleTimeSlicing;
    [SerializeField] private Toggle toggleGreenComputing;

    [Header("Audio Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // PlayerPrefs keys
    private const string KEY_POOLING = "useObjectPooling";
    private const string KEY_SPATIAL = "useSpatialPartitioning";
    private const string KEY_TIMESLICE = "useTimeSlicing";
    private const string KEY_GREEN = "useGreenComputing";
    private const string KEY_MUSIC = "musicVolume";
    private const string KEY_SFX = "sfxVolume";

    private void OnEnable()
    {
        LoadSettings();
    }

    // Reads saved values from PlayerPrefs and syncs the UI
    private void LoadSettings()
    {
        if (optimizationSettings != null)
        {
            bool pooling = PlayerPrefs.GetInt(KEY_POOLING, optimizationSettings.useObjectPooling ? 1 : 0) == 1;
            bool spatial = PlayerPrefs.GetInt(KEY_SPATIAL, optimizationSettings.useSpatialPartitioning ? 1 : 0) == 1;
            bool timeslice = PlayerPrefs.GetInt(KEY_TIMESLICE, optimizationSettings.useTimeSlicing ? 1 : 0) == 1;
            bool green = PlayerPrefs.GetInt(KEY_GREEN, optimizationSettings.useGreenComputing ? 1 : 0) == 1;

            // Apply to ScriptableObject so game scene reads correct values
            optimizationSettings.useObjectPooling = pooling;
            optimizationSettings.useSpatialPartitioning = spatial;
            optimizationSettings.useTimeSlicing = timeslice;
            optimizationSettings.useGreenComputing = green;

            // Sync UI without triggering OnValueChanged
            toggleObjectPooling.SetIsOnWithoutNotify(pooling);
            toggleSpatialPartitioning.SetIsOnWithoutNotify(spatial);
            toggleTimeSlicing.SetIsOnWithoutNotify(timeslice);
            toggleGreenComputing.SetIsOnWithoutNotify(green);
        }

        float music = PlayerPrefs.GetFloat(KEY_MUSIC, 0.5f);
        float sfx = PlayerPrefs.GetFloat(KEY_SFX, 1f);

        musicSlider.SetValueWithoutNotify(music);
        sfxSlider.SetValueWithoutNotify(sfx);

        if (AudioManager.Instance != null && AudioManager.Instance.IsReady)
        {
            AudioManager.Instance.SetMusicVolume(music);
            AudioManager.Instance.SetSFXVolume(sfx);
        }
    }

    public void OnObjectPoolingToggle(bool value)
    {
        optimizationSettings.useObjectPooling = value;
        PlayerPrefs.SetInt(KEY_POOLING, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnSpatialPartitioningToggle(bool value)
    {
        optimizationSettings.useSpatialPartitioning = value;
        PlayerPrefs.SetInt(KEY_SPATIAL, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnTimeSlicingToggle(bool value)
    {
        optimizationSettings.useTimeSlicing = value;
        PlayerPrefs.SetInt(KEY_TIMESLICE, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnGreenComputingToggle(bool value)
    {
        optimizationSettings.useGreenComputing = value;
        Application.targetFrameRate = value ? optimizationSettings.targetFrameRate : -1;
        PlayerPrefs.SetInt(KEY_GREEN, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat(KEY_MUSIC, value);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat(KEY_SFX, value);
        PlayerPrefs.Save();
    }

    public void OnFPSToggle(bool value)
    {
        if(AFPSCounter.Instance == null) return;
        AFPSCounter.Instance.enabled = value;
        PlayerPrefs.SetInt("showFPSCounter", value ? 1 : 0);
        PlayerPrefs.Save();
    }
}
