using UnityEngine;
using TMPro;  // Import TextMeshPro namespace

public class SteeringModeManager : MonoBehaviour
{
    public TMP_Dropdown steeringDropdown;  // Reference to TMP_Dropdown
    private SteeringModeCarController steeringCarController;  // Reference to the SteeringModeCarController script

    void Start()
    {
        // Find the SteeringModeCarController object in the scene (it should persist across scenes)
        steeringCarController = FindObjectOfType<SteeringModeCarController>();

        // If SteeringModeCarController is not found, log an error
        if (steeringCarController == null)
        {
            Debug.LogError("SteeringModeCarController not found. Ensure it is set to Don't Destroy On Load.");
        }

        // Add listener for dropdown value change
        steeringDropdown.onValueChanged.AddListener(OnSteeringModeChanged);
    }

    // This function will be called when the dropdown value changes
    void OnSteeringModeChanged(int index)
    {
        if (index == 0)  // If Touch is selected
        {
            steeringCarController.SetSteeringMode(SteeringMode.Touch);
        }
        else if (index == 1)  // If Tilt is selected
        {
            steeringCarController.SetSteeringMode(SteeringMode.Tilt);
        }
    }
}
