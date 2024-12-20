using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettings : MonoBehaviour
{
    public AudioSource backgroundMusic; // Background music source
    public AudioSource[] soundEffects; // Array of sound effects

    public Slider musicVolumeSlider; // Slider for music volume
    public Slider soundEffectsVolumeSlider; // Slider for SFX volume

    public Button muteButton; // Button for muting
    public TextMeshProUGUI muteButtonText; // Text for mute button

    public TextMeshProUGUI musicVolumeText; // Text for music volume
    public TextMeshProUGUI soundEffectsVolumeText; // Text for SFX volume

    private bool isMuted = false; // Tracks mute state

    void Start()
    {
        // Initialize volume and text
        musicVolumeSlider.value = backgroundMusic.volume;
        UpdateMusicVolumeText();

        float avgSFXVolume = 0f;
        foreach (var effect in soundEffects)
        {
            avgSFXVolume += effect.volume;
        }
        soundEffectsVolumeSlider.value = (soundEffects.Length > 0) ? avgSFXVolume / soundEffects.Length : 0f;
        UpdateSoundEffectsVolumeText();

        // Add listeners to sliders and mute button
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        soundEffectsVolumeSlider.onValueChanged.AddListener(OnSoundEffectsVolumeChanged);
        muteButton.onClick.AddListener(ToggleMute);

        UpdateMuteButtonText();
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (!isMuted)
        {
            backgroundMusic.volume = value; // Update background music volume
            UpdateMusicVolumeText();
        }
    }

    public void OnSoundEffectsVolumeChanged(float value)
    {
        if (!isMuted)
        {
            foreach (var effect in soundEffects)
            {
                effect.volume = value; // Update sound effects volume
            }
            UpdateSoundEffectsVolumeText();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted; // Toggle mute state

        if (isMuted)
        {
            // Mute background music and sound effects by setting their volume to 0
            if (backgroundMusic != null)
            {
                backgroundMusic.volume = 0f;
            }

            foreach (var effect in soundEffects)
            {
                if (effect != null)
                {
                    effect.volume = 0f;
                }
            }
        }
        else
        {
            // Restore volumes to slider values
            if (backgroundMusic != null)
            {
                backgroundMusic.volume = musicVolumeSlider.value;
            }

            foreach (var effect in soundEffects)
            {
                if (effect != null)
                {
                    effect.volume = soundEffectsVolumeSlider.value;
                }
            }
        }

        // Update UI elements
        UpdateMuteButtonText();
    }

    private void UpdateMusicVolumeText()
    {
        musicVolumeText.text = $"{Mathf.RoundToInt(musicVolumeSlider.value * 100)}%"; // Show only percentage
    }

    private void UpdateSoundEffectsVolumeText()
    {
        soundEffectsVolumeText.text = $"{Mathf.RoundToInt(soundEffectsVolumeSlider.value * 100)}%"; // Show only percentage
    }

    private void UpdateMuteButtonText()
    {
        muteButtonText.text = isMuted ? "Unmute" : "Mute"; // Toggle button text
    }
}
