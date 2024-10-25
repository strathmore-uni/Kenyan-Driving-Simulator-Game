using UnityEngine;
using UnityEngine.UI;

public class GearButtonController : MonoBehaviour
{
    // Array of all gear buttons
    public Button[] gearButtons;

    // Index to track the currently visible gear button
    private int currentGearIndex = 0;

    void Start()
    {
        // Initially show the first gear button and hide others
        UpdateGearButtonVisibility();

        // Add listeners to all buttons (you might need to do this in the inspector as well)
        foreach (Button gearButton in gearButtons)
        {
            gearButton.onClick.AddListener(OnGearButtonClick);
        }
    }

    // This method is called when any of the gear buttons are clicked
    void OnGearButtonClick()
    {
        // Hide the current gear button
        gearButtons[currentGearIndex].gameObject.SetActive(false);

        // Move to the next gear
        currentGearIndex = (currentGearIndex + 1) % gearButtons.Length;

        // Show the next gear button
        gearButtons[currentGearIndex].gameObject.SetActive(true);

        // Optional: Update the car's gear (if needed)
        // You can use the index or name of the button to set the car's gear
        // Example: carManager.SetGear(gearButtons[currentGearIndex].name);
    }

    // Update the visibility of all buttons
    void UpdateGearButtonVisibility()
    {
        // Hide all gear buttons
        foreach (Button gearButton in gearButtons)
        {
            gearButton.gameObject.SetActive(false);
        }

        // Show the first gear button
        gearButtons[currentGearIndex].gameObject.SetActive(true);
    }
}
