using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyNamespace;

public class ControlSettings : MonoBehaviour
{
    // Add references to the UI elements
    public Slider steeringSensitivitySlider;
    public Slider pedalSensitivitySlider;
    public TMP_Text steeringSensitivityText;
    public TMP_Text pedalSensitivityText;

    // Reference to the CarManager (your car's movement and steering script)
    public CarManager carManager;

    private void Start()
    {
        // Initialize the sliders with the current sensitivity values from the CarManager
        steeringSensitivitySlider.value = carManager.steeringSensitivity;
        pedalSensitivitySlider.value = carManager.accelerationSensitivity;

        // Initialize the text values
        steeringSensitivityText.text = "Steering Sensitivity: " + steeringSensitivitySlider.value.ToString("0.0");
        pedalSensitivityText.text = "Pedal Sensitivity: " + pedalSensitivitySlider.value.ToString("0.0");

        // Add listeners to sliders to update sensitivity values
        steeringSensitivitySlider.onValueChanged.AddListener(SetSteeringSensitivity);
        pedalSensitivitySlider.onValueChanged.AddListener(SetPedalSensitivity);
    }

    // Method to update steering sensitivity in CarManager
    public void SetSteeringSensitivity(float value)
    {
        carManager.steeringSensitivity = value;
        steeringSensitivityText.text = "Steering Sensitivity: " + value.ToString("0.0");
    }

    // Method to update pedal sensitivity in CarManager
    public void SetPedalSensitivity(float value)
    {
        carManager.accelerationSensitivity = value;
        pedalSensitivityText.text = "Pedal Sensitivity: " + value.ToString("0.0");
    }

    // Optionally: Save settings (e.g., PlayerPrefs) for persistent settings across scenes
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SteeringSensitivity", carManager.steeringSensitivity);
        PlayerPrefs.SetFloat("PedalSensitivity", carManager.accelerationSensitivity);
        PlayerPrefs.Save();
    }

    // Optionally: Load settings on start
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("SteeringSensitivity"))
        {
            carManager.steeringSensitivity = PlayerPrefs.GetFloat("SteeringSensitivity");
            steeringSensitivitySlider.value = carManager.steeringSensitivity;
        }
        if (PlayerPrefs.HasKey("PedalSensitivity"))
        {
            carManager.accelerationSensitivity = PlayerPrefs.GetFloat("PedalSensitivity");
            pedalSensitivitySlider.value = carManager.accelerationSensitivity;
        }
    }
}
