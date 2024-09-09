using MyNamespace;
using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class InteriorSteeringControl : MonoBehaviour
{
    public Transform interiorSteeringWheel;  // Transform of the interior steering wheel
    public float maxSteeringAngle = 450f;    // Maximum rotation angle for the interior steering wheel
    public float steeringSensitivity = 10f;  // Sensitivity for steering input
    public Button switchViewButton;          // Button to switch views

    private bool isInteriorView = false;     // Track if the interior view is active

    void Start()
    {
        // Assign the button click event
        switchViewButton.onClick.AddListener(SwitchToInteriorView);
    }

    void Update()
    {
        if (isInteriorView)
        {
            // Handle steering input for interior view
            HandleInteriorSteering();
        }
        else
        {
            // Reset the interior steering wheel to a neutral position if not in interior view
            ResetSteeringWheel();
        }
    }

    private void HandleInteriorSteering()
    {
        // Get input for the steering wheel (e.g., from keyboard or touch)
        float steeringInput = Input.GetAxis("Horizontal");

        // Calculate the target rotation angle for the steering wheel
        float targetSteeringAngle = maxSteeringAngle * steeringInput;

        // Smoothly rotate the steering wheel to match input
        interiorSteeringWheel.localRotation = Quaternion.Lerp(interiorSteeringWheel.localRotation, Quaternion.Euler(0, 0, -targetSteeringAngle), Time.deltaTime * steeringSensitivity);

        // Apply steering to the car
        ApplySteeringToCar(steeringInput);
    }

    private void ResetSteeringWheel()
    {
        // Smoothly reset the steering wheel to the center
        interiorSteeringWheel.localRotation = Quaternion.Lerp(interiorSteeringWheel.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * steeringSensitivity);
    }

    private void ApplySteeringToCar(float input)
    {
        // Correctly get the CarManager component
        CarManager carManager = GetComponent<CarManager>();  // Notice the change here: carManager (variable) vs. CarManager (class)
        if (carManager != null)
        {
            carManager.Steer(input);  // Assuming your CarManager script has a Steer method
        }
        else
        {
            Debug.LogWarning("CarManager component not found on this GameObject.");
        }
    }


    // Method to switch to interior view
    public void SwitchToInteriorView()
    {
        isInteriorView = !isInteriorView; // Toggle the interior view state
    }
}
