using UnityEngine;
using UnityEngine.UI;

public class GearShiftDropdownButtons : MonoBehaviour
{
    public Button mainButton;         // The main button to toggle the dropdown
    public Button parkButton;         // Button for Park (P)
    public Button reverseButton;      // Button for Reverse (R)
    public Button neutralButton;      // Button for Neutral (N)
    public Button driveButton;        // Button for Drive (D)
    private bool dropdownVisible = false; // To track visibility of buttons

    private Rigidbody carRigidbody;
    private string currentGear = "P"; // Default gear

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();

        // Add listeners for the main button and gear buttons
        mainButton.onClick.AddListener(ToggleDropdown);
        parkButton.onClick.AddListener(() => SelectGear("P"));
        reverseButton.onClick.AddListener(() => SelectGear("R"));
        neutralButton.onClick.AddListener(() => SelectGear("N"));
        driveButton.onClick.AddListener(() => SelectGear("D"));

        // Hide the gear buttons at the start
        HideGearButtons();
    }

    void ToggleDropdown()
    {
        if (dropdownVisible)
        {
            HideGearButtons();  // Hide the dropdown
        }
        else
        {
            ShowGearButtons();  // Show the dropdown
        }
    }

    void ShowGearButtons()
    {
        parkButton.gameObject.SetActive(true);
        reverseButton.gameObject.SetActive(true);
        neutralButton.gameObject.SetActive(true);
        driveButton.gameObject.SetActive(true);
        dropdownVisible = true;
    }

    void HideGearButtons()
    {
        parkButton.gameObject.SetActive(false);
        reverseButton.gameObject.SetActive(false);
        neutralButton.gameObject.SetActive(false);
        driveButton.gameObject.SetActive(false);
        dropdownVisible = false;
    }

    void SelectGear(string gear)
    {
        currentGear = gear;
        HideGearButtons();  // Hide the buttons after selecting a gear
        Debug.Log("Current Gear: " + currentGear);

        // Logic to handle gear behavior based on the selected gear
        switch (currentGear)
        {
            case "P":
                carRigidbody.velocity = Vector3.zero;
                Debug.Log("Gear: Park");
                break;
            case "R":
                carRigidbody.velocity = -transform.forward * 5f; // Reverse logic
                Debug.Log("Gear: Reverse");
                break;
            case "N":
                carRigidbody.velocity = Vector3.zero; // Neutral logic
                Debug.Log("Gear: Neutral");
                break;
            case "D":
                carRigidbody.velocity = transform.forward * 5f; // Drive logic
                Debug.Log("Gear: Drive");
                break;
        }
    }
}
