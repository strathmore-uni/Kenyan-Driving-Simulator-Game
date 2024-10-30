using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarGearController : MonoBehaviour
{
    public Button gearButton; // Reference to the button used to change gears
    private int currentGear = 0; // 0 = Park, 1 = Drive, 2 = Reverse, 3 = Neutral
    public float speed = 10f; // Speed of the car
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        gearButton.onClick.AddListener(CycleGear); // Add listener to the button
        UpdateGearDisplay(); // Update display at the start
    }

    private void Update()
    {
        MoveCar(); // Call the movement function
    }

    private void CycleGear()
    {
        currentGear = (currentGear + 1) % 4; // Cycle through 0 to 3
        UpdateGearDisplay(); // Update button text to reflect current gear
        Debug.Log("Gear changed to: " + currentGear); // Log gear changes
    }

    private void MoveCar()
    {
        switch (currentGear)
        {
            case 1: // Drive
                rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
                break;
            case 2: // Reverse
                rb.MovePosition(transform.position - transform.forward * speed * Time.deltaTime);
                break;
            case 0: // Park
            case 3: // Neutral
                // No movement for Park and Neutral
                break;
            default:
                break;
        }
    }

    private void UpdateGearDisplay()
    {
        // Update the button's text to reflect the current gear using TextMeshProUGUI
        TextMeshProUGUI buttonText = gearButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            switch (currentGear)
            {
                case 0:
                    buttonText.text = "P (Park)";
                    break;
                case 1:
                    buttonText.text = "D (Drive)";
                    break;
                case 2:
                    buttonText.text = "R (Reverse)";
                    break;
                case 3:
                    buttonText.text = "N (Neutral)";
                    break;
                default:
                    break;
            }
        }
    }
}
