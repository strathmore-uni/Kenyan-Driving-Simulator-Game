using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MuteManager : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusic; // Background music source
    [SerializeField] private AudioSource[] soundEffects;  // Array of sound effects
    [SerializeField] private Button muteButton;           // Button to toggle mute
    [SerializeField] private TextMeshProUGUI muteButtonText; // Text for mute button

    private bool isMuted = false; // Tracks the mute state

    void Start()
    {
        // Add listener for the mute button
        muteButton.onClick.AddListener(ToggleMute);

        // Update the initial text
        UpdateMuteButtonText();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted; // Toggle mute state

        if (isMuted)
        {
            // Mute all audio sources
            if (backgroundMusic != null)
            {
                backgroundMusic.Pause(); // Pause background music
            }

            if (soundEffects != null)
            {
                foreach (var effect in soundEffects)
                {
                    effect.Pause(); // Pause each sound effect
                }
            }
        }
        else
        {
            // Unmute all audio sources
            if (backgroundMusic != null)
            {
                backgroundMusic.UnPause(); // Resume background music
            }

            if (soundEffects != null)
            {
                foreach (var effect in soundEffects)
                {
                    effect.UnPause(); // Resume each sound effect
                }
            }
        }

        // Update button text
        UpdateMuteButtonText();
    }

    private void UpdateMuteButtonText()
    {
        muteButtonText.text = isMuted ? "Unmute" : "Mute";
    }
}
