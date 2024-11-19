using UnityEngine;
using UnityEngine.UI; // To use UI components like Dropdown

public class ControlModeSwitcher : MonoBehaviour
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

    // Reference to the dropdown UI element
    public Dropdown modeDropdown;

    void Start()
    {
        // Add listener to handle dropdown value change
        modeDropdown.onValueChanged.AddListener(OnModeChanged);
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

        // Apply the control settings after change
        ApplyControlSettings();
    }

    // Method to apply the control settings
    void ApplyControlSettings()
    {
        // Here, you can apply logic to enable the appropriate control based on the selected mode
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
                // Enable Touch-based gas pedal control
                break;
            case ControlMode.Joystick:
                // Enable Joystick-based gas pedal control
                break;
        }

        switch (brakeMode)
        {
            case ControlMode.Touch:
                // Enable Touch-based brake pedal control
                break;
            case ControlMode.Joystick:
                // Enable Joystick-based brake pedal control
                break;
        }
    }
}
