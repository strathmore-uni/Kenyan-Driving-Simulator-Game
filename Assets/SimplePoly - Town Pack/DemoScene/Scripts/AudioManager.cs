using UnityEngine;
using UnityEngine.Audio; // Import the Audio namespace
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider volumeSlider;      // Slider to control volume
    public AudioMixer audioMixer;    // Reference to the AudioMixer
    public string exposedParameter = "MasterVolume"; // Name of the exposed parameter in the mixer

    void Start()
    {
        // Initialize the slider value to saved setting or default to max volume
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.onValueChanged.AddListener(SetVolume);  // Set volume when slider changes
    }

    // Update the volume in the AudioMixer
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(exposedParameter, Mathf.Log10(volume) * 20);  // Adjust volume in dB
        PlayerPrefs.SetFloat("Volume", volume);  // Save setting
    }
}
