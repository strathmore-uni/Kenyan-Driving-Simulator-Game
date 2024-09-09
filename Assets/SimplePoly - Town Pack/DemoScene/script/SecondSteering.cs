using UnityEngine;

public class SteeringController : MonoBehaviour
{
    // Reference to the steering wheel game object
    public GameObject steeringWheel;

    // Reference to the car's Rigidbody component
    public Rigidbody carRigidbody;

    // Sensitivity of the steering
    public float steeringSensitivity = 5.0f;

    // Minimum and maximum rotation angles for the steering wheel
    public float minRotation = -30.0f;
    public float maxRotation = 30.0f;

    // Speed of the car's rotation
    public float carRotationSpeed = 5.0f;

    private Vector3 initialMousePosition;
    private float initialSteeringWheelRotation;
    private float initialCarRotation;
    private float targetCarRotation;

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Store the initial mouse position, steering wheel rotation, and car rotation
            initialMousePosition = Input.mousePosition;
            initialSteeringWheelRotation = steeringWheel.transform.eulerAngles.y;
            initialCarRotation = carRigidbody.transform.eulerAngles.y;
        }

        // Check if the left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            // Calculate the mouse movement delta
            Vector3 mouseDelta = Input.mousePosition - initialMousePosition;

            // Calculate the new steering wheel rotation
            float newSteeringWheelRotation = initialSteeringWheelRotation + (mouseDelta.x * steeringSensitivity);

            // Clamp the steering wheel rotation to the minimum and maximum angles
            newSteeringWheelRotation = Mathf.Clamp(newSteeringWheelRotation, minRotation, maxRotation);

            // Update the steering wheel rotation
            steeringWheel.transform.eulerAngles = new Vector3(0, newSteeringWheelRotation, 0);

            // Calculate the new car rotation based on the steering wheel rotation
            targetCarRotation = initialCarRotation + (newSteeringWheelRotation - initialSteeringWheelRotation);
        }

        // Smoothly rotate the car towards the target rotation
        carRigidbody.MoveRotation(Quaternion.Lerp(carRigidbody.rotation, Quaternion.Euler(0, targetCarRotation, 0), carRotationSpeed * Time.deltaTime));

        // Check if the interior button is pressed
        if (Input.GetButtonDown("InteriorButton"))
        {
            // Set the target car rotation to a new value (e.g. 10 degrees to the right)
            targetCarRotation = initialCarRotation + 10f;

            // Smoothly rotate the car towards the target rotation
            carRigidbody.MoveRotation(Quaternion.Lerp(carRigidbody.rotation, Quaternion.Euler(0, targetCarRotation, 0), carRotationSpeed * Time.deltaTime * 0.5f));
        }
    }
}