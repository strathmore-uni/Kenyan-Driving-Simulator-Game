using UnityEngine;
using TMPro; // For TMP_Dropdown (TextMeshPro)

public class ControlModeManager : MonoBehaviour
{
    // Enum for Control Modes
    public enum ControlMode
    {
        Touch,
        Tilt,
        Joystick
    }

    // Static variables to store the current mode for each control
    public static ControlMode steeringMode = ControlMode.Touch;
    public static ControlMode gasMode = ControlMode.Touch;
    public static ControlMode brakeMode = ControlMode.Touch;

    // Reference to the TMP_Dropdown UI element (TextMeshPro version)
    public TMP_Dropdown modeDropdown;

    void Start()
    {
        // Load saved control settings
        LoadControlSettings();

        // Set dropdown to the current saved value
        modeDropdown.value = (int)steeringMode;

        // Add listener to handle dropdown value change
        modeDropdown.onValueChanged.AddListener(OnModeChanged);

        // Optionally, you can auto-apply controls on start for gameplay if needed
        ApplyControlSettings();
    }

    // This method is called when the dropdown value changes
    void OnModeChanged(int index)
    {
        // Map the dropdown index to the ControlMode enum
        ControlMode selectedMode = (ControlMode)index;

        // Set all control modes to the selected value
        steeringMode = selectedMode;
        gasMode = selectedMode;
        brakeMode = selectedMode;

        // Save the updated settings to PlayerPrefs
        SaveControlSettings();

        // Apply the control settings after change
        ApplyControlSettings();
    }

    // Save control settings to PlayerPrefs
    void SaveControlSettings()
    {
        PlayerPrefs.SetInt("SteeringMode", (int)steeringMode);
        PlayerPrefs.SetInt("GasMode", (int)gasMode);
        PlayerPrefs.SetInt("BrakeMode", (int)brakeMode);
        PlayerPrefs.Save();
    }

    // Load control settings from PlayerPrefs
    void LoadControlSettings()
    {
        if (PlayerPrefs.HasKey("SteeringMode"))
        {
            steeringMode = (ControlMode)PlayerPrefs.GetInt("SteeringMode");
            gasMode = (ControlMode)PlayerPrefs.GetInt("GasMode");
            brakeMode = (ControlMode)PlayerPrefs.GetInt("BrakeMode");
        }
    }

    // Apply the control settings in gameplay
    void ApplyControlSettings()
    {
        // Here, you can implement logic to apply the selected mode for controls
        switch (steeringMode)
        {
            case ControlMode.Touch:
                // Enable Touch-based steering
                break;
            case ControlMode.Tilt:
                // Enable Tilt-based steering
                break;
            case ControlMode.Joystick:
                // Enable Joystick-based steering
                break;
        }

        switch (gasMode)
        {
            case ControlMode.Touch:
                // Enable Touch-based gas pedal
                break;
            case ControlMode.Joystick:
                // Enable Joystick-based gas pedal
                break;
        }

        switch (brakeMode)
        {
            case ControlMode.Touch:
                // Enable Touch-based brake pedal
                break;
            case ControlMode.Joystick:
                // Enable Joystick-based brake pedal
                break;
        }
    }
}
