using UnityEngine;
using UnityEngine.UI;  // For accessing UI Button

public class ReverseGearScript : MonoBehaviour
{
    public float reverseForce = 3000f;  // Force applied when reversing
    public float maxReverseSpeed = 10f; // Maximum speed when reversing
    public Button reverseGearButton;    // Reference to the reverse gear button (UI button)

    private Rigidbody carRigidbody;
    private bool isInReverseGear = false; // Tracks if the car is in reverse gear

    void Start()
    {
        // Initialize the Rigidbody component
        carRigidbody = GetComponent<Rigidbody>();

        // Ensure the reverse button triggers the method when clicked
        reverseGearButton.onClick.AddListener(EnterReverseGear);
    }

    void Update()
    {
        // You can check for additional conditions to exit reverse, like if another button is pressed.
        // You can use another button to deactivate reverse gear or any other logic.
        if (Input.GetKeyDown(KeyCode.P)) // Assuming 'P' key is used for neutral (or park)
        {
            ExitReverseGear();
        }
    }

    void FixedUpdate()
    {
        if (isInReverseGear)
        {
            HandleReverseMovement();
        }
    }

    // Method to activate reverse gear when button is clicked
    public void EnterReverseGear()
    {
        if (!isInReverseGear)
        {
            isInReverseGear = true;
            Debug.Log("Entering Reverse Gear");
        }
    }

    // Method to deactivate reverse gear
    void ExitReverseGear()
    {
        if (isInReverseGear)
        {
            isInReverseGear = false;
            Debug.Log("Exiting Reverse Gear");
        }
    }

    // Handle reverse movement when in reverse gear
    void HandleReverseMovement()
    {
        float input = Input.GetAxis("Vertical"); // Assuming vertical axis for gas pedal input (W/S or Up/Down arrow)

        // Apply reverse force when pressing the gas pedal (positive input)
        if (input > 0)
        {
            carRigidbody.AddForce(-transform.forward * input * reverseForce, ForceMode.Acceleration);
        }

        // Limit speed when reversing
        if (carRigidbody.velocity.magnitude > maxReverseSpeed)
        {
            carRigidbody.velocity = carRigidbody.velocity.normalized * maxReverseSpeed;
        }
    }
}
