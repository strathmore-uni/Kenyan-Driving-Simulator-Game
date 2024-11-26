using UnityEngine;
using UnityEngine.UI; // Include for Button support

public class ReverseGear : MonoBehaviour
{
    public float moveSpeed = 10f; // Forward speed (used for drive gear)
    public float reverseSpeed = 5f; // Speed when in reverse gear
    public Rigidbody rb;
    private bool isReversing = false; // Flag to check if the car is in reverse gear
    private float currentThrottle = 0f; // Throttle value for gas pedal input (0-1 range)

    public Button reverseButton; // Reference to the Reverse button in the UI

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Add listener to reverse button to shift to reverse gear
        reverseButton.onClick.AddListener(ShiftToReverse);
    }

    void Update()
    {
        // Handle gear shift and movement
        if (isReversing)
        {
            HandleReverseMovement();
        }
        else
        {
            StopCar();
        }
    }

    void HandleReverseMovement()
    {
        // Get the throttle input (gas pedal)
        currentThrottle = Mathf.Clamp01(Input.GetAxis("Vertical")); // assuming the "Vertical" axis is used for throttle (W/S or Up/Down arrows)

        // Apply reverse force if throttle is pressed (gas pedal is down)
        if (currentThrottle > 0)
        {
            rb.velocity = transform.forward * -reverseSpeed * currentThrottle; // Negative to move backward
        }
        else
        {
            StopCar(); // Stop car if throttle is not pressed
        }
    }

    void StopCar()
    {
        // Stop the car completely when not in reverse or when gas pedal is released
        rb.velocity = Vector3.zero;
    }

    // Call this function to shift to reverse gear
    public void ShiftToReverse()
    {
        isReversing = true;
        Debug.Log("Reverse gear engaged.");
    }

    // Call this function to stop the reverse gear (e.g., when shifting out of reverse)
    public void ShiftOutOfReverse()
    {
        isReversing = false;
        StopCar();
        Debug.Log("Exited reverse gear.");
    }
}
