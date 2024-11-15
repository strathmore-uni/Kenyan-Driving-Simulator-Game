using UnityEngine;

public enum SteeringMode { Touch, Tilt }

public class SteeringModeCarController : MonoBehaviour
{
    // Current steering mode (set from dropdown or other UI)
    public SteeringMode currentSteeringMode = SteeringMode.Touch;

    // Steering input values
    private float steeringInput;
    private float tiltInput;

    // Speed at which steering will apply
    public float steeringSpeed = 2f;

    void Awake()
    {
        // Ensure the car controller persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Apply steering based on the current mode
        if (currentSteeringMode == SteeringMode.Touch)
        {
            HandleTouchSteering();
        }
        else if (currentSteeringMode == SteeringMode.Tilt)
        {
            HandleTiltSteering();
        }

        ApplySteering();
    }

    // Handle touch steering input (touch position on the screen)
    void HandleTouchSteering()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            steeringInput = (touch.position.x / Screen.width) - 0.5f; // Normalize to range [-0.5, 0.5]
        }
    }

    // Handle tilt steering input (using device accelerometer for tilt input)
    void HandleTiltSteering()
    {
        tiltInput = Input.acceleration.x; // X axis tilt
        steeringInput = Mathf.Clamp(tiltInput, -1f, 1f); // Normalize to [-1, 1]
    }

    // Apply the steering to the car's transform (rotate the car)
    void ApplySteering()
    {
        float currentAngle = steeringInput * steeringSpeed; // Adjust steering speed
        transform.Rotate(0, currentAngle, 0); // Rotate around the Y axis (car steering)
    }

    // This method is called to update the steering mode from other scripts
    public void SetSteeringMode(SteeringMode mode)
    {
        currentSteeringMode = mode;
    }
}
