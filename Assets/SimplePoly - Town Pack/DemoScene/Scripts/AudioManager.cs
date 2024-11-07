using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundMusicAudioSource;  // Background music AudioSource
    public AudioSource soundEffectsAudioSource;     // Sound effects AudioSource (engine, collision, etc.)

    [Header("UI Elements")]
    public Slider bgMusicVolumeSlider;              // Slider for background music volume
    public Slider soundEffectsVolumeSlider;         // Slider for sound effects volume

    private void Start()
    {
        // Load saved settings, defaulting to 1.0f if settings are not found
        bgMusicVolumeSlider.value = PlayerPrefs.GetFloat("BGMusicVolume", 1.0f);
        soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 1.0f);

        // Apply loaded values to the AudioSources
        SetBGMusicVolume(bgMusicVolumeSlider.value);
        SetSoundEffectsVolume(soundEffectsVolumeSlider.value);

        // Add listeners to update volume when slider values change
        bgMusicVolumeSlider.onValueChanged.AddListener(SetBGMusicVolume);
        soundEffectsVolumeSlider.onValueChanged.AddListener(SetSoundEffectsVolume);
    }

    // Method to set background music volume and save it
    public void SetBGMusicVolume(float volume)
    {
        backgroundMusicAudioSource.volume = volume;
        PlayerPrefs.SetFloat("BGMusicVolume", volume);  // Save volume setting
    }

    // Method to set sound effects volume and save it
    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsAudioSource.volume = volume;
        PlayerPrefs.SetFloat("SoundEffectsVolume", volume);  // Save volume setting
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();  // Ensure settings are saved when the game exits
    }
}
