using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettings : MonoBehaviour
{
    public AudioSource backgroundMusic; // Background music source
    public AudioSource[] soundEffects; // Array of sound effects

    public Slider musicVolumeSlider; // Slider for music volume
    public Slider soundEffectsVolumeSlider; // Slider for SFX volume

    public TextMeshProUGUI musicVolumeText; // Text for music volume
    public TextMeshProUGUI soundEffectsVolumeText; // Text for SFX volume

    private int musicVolume; // Integer to store music volume as percentage
    private int soundEffectsVolume; // Integer to store SFX volume as percentage

    void Start()
    {
        // Initialize music volume
        musicVolume = Mathf.RoundToInt(backgroundMusic.volume * 100); // Convert initial volume to percentage
        musicVolumeSlider.value = backgroundMusic.volume;
        UpdateMusicVolumeText();

        // Initialize SFX volume
        float avgSFXVolume = 0f;
        foreach (var effect in soundEffects)
        {
            avgSFXVolume += effect.volume;
        }
        soundEffectsVolume = Mathf.RoundToInt((soundEffects.Length > 0 ? avgSFXVolume / soundEffects.Length : 0f) * 100);
        soundEffectsVolumeSlider.value = soundEffects.Length > 0 ? avgSFXVolume / soundEffects.Length : 0f;
        UpdateSoundEffectsVolumeText();

        // Add listeners to sliders
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        soundEffectsVolumeSlider.onValueChanged.AddListener(OnSoundEffectsVolumeChanged);
    }

    public void OnMusicVolumeChanged(float value)
    {
        musicVolume = Mathf.RoundToInt(value * 100); // Convert slider value to integer percentage
        backgroundMusic.volume = value; // Set the actual volume
        UpdateMusicVolumeText();
    }

    public void OnSoundEffectsVolumeChanged(float value)
    {
        soundEffectsVolume = Mathf.RoundToInt(value * 100); // Convert slider value to integer percentage
        foreach (var effect in soundEffects)
        {
            effect.volume = value; // Update volume for each SFX
        }
        UpdateSoundEffectsVolumeText();
    }

    private void UpdateMusicVolumeText()
    {
        musicVolumeText.text = $"{musicVolume}%"; // Update music volume text to show only the percentage
    }

    private void UpdateSoundEffectsVolumeText()
    {
        soundEffectsVolumeText.text = $"{soundEffectsVolume}%"; // Update SFX volume text to show only the percentage
    }
}
