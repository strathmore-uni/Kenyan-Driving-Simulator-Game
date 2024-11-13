using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettings : MonoBehaviour
{
    public AudioSource backgroundMusic; // Background music audio source
    public AudioSource[] soundEffects; // Array of sound effects audio sources

    // UI sliders and mute button
    public Slider musicVolumeSlider;
    public Slider soundEffectsVolumeSlider;
    public Button muteButton;
    public TextMeshProUGUI muteButtonText;

    // Volume display texts
    public TextMeshProUGUI musicVolumeText; // Display text for music volume
    public TextMeshProUGUI soundEffectsVolumeText; // Display text for sound effects volume

    private bool isMuted = false;

    void Start()
    {
        // Set the sliders to match the current volume
        musicVolumeSlider.value = backgroundMusic.volume;

        // Set the sound effects volume slider to an average of all sound effects
        float avgSFXVolume = 0f;
        foreach (var effect in soundEffects)
        {
            avgSFXVolume += effect.volume;
        }
        soundEffectsVolumeSlider.value = (soundEffects.Length > 0) ? avgSFXVolume / soundEffects.Length : 0f;

        // Add listeners for mute button and volume sliders
        muteButton.onClick.AddListener(ToggleMute);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        soundEffectsVolumeSlider.onValueChanged.AddListener(OnSoundEffectsVolumeChanged);

        // Set the initial mute button text and update volume display text
        UpdateMuteButtonText();
        UpdateVolumeTexts();
    }

    // Method to adjust background music volume
    public void OnMusicVolumeChanged(float value)
    {
        if (!isMuted)
        {
            backgroundMusic.volume = value;
            musicVolumeText.text = $"Music Volume: {(int)(value * 100)}%";
        }
    }

    // Method to adjust sound effects volume
    public void OnSoundEffectsVolumeChanged(float value)
    {
        if (!isMuted)
        {
            foreach (var effect in soundEffects)
            {
                effect.volume = value;
            }
            soundEffectsVolumeText.text = $"SFX Volume: {(int)(value * 100)}%";
        }
    }

    // Method to mute/unmute background music and sound effects
    public void ToggleMute()
    {
        isMuted = !isMuted;

        // Mute or unmute background music
        backgroundMusic.mute = isMuted;

        // Mute or unmute all sound effects
        foreach (var effect in soundEffects)
        {
            effect.mute = isMuted;
        }

        // Update the button text to reflect the mute state
        UpdateMuteButtonText();
    }

    // Update the mute button's text
    private void UpdateMuteButtonText()
    {
        muteButtonText.text = isMuted ? "Unmute" : "Mute";
    }

    // Method to initialize volume texts on start
    private void UpdateVolumeTexts()
    {
        musicVolumeText.text = $"Music Volume: {(int)(musicVolumeSlider.value * 100)}%";
        soundEffectsVolumeText.text = $"SFX Volume: {(int)(soundEffectsVolumeSlider.value * 100)}%";
    }
}
