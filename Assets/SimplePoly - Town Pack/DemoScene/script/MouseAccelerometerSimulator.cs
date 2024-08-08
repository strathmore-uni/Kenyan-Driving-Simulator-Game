using UnityEngine;

public class MouseAccelerometerSimulator : MonoBehaviour
{
    public float sensitivity = 5.0f; // adjust this value to control the sensitivity of the mouse input
    public float speed = 10.0f; // adjust this value to control the speed of the car
    public float rotationSpeed = 5.0f; // adjust this value to control the rotation speed of the car

    private Rigidbody rb;
    private float mouseXPrev;
    private float mouseYPrev;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get the mouse position
        Vector2 mousePosition = Input.mousePosition;

        // Calculate the mouse movement
        float mouseX = mousePosition.x - mouseXPrev;
        float mouseY = mousePosition.y - mouseYPrev;

        // Simulate the accelerometer X-axis input (left/right)
        float accelerometerX = mouseX * sensitivity;

        // Simulate the accelerometer Y-axis input (forward/backward)
        float accelerometerY = 0;
        if (Input.GetMouseButton(0)) // left mouse button
        {
            accelerometerY = 1; // forward
        }
        else if (Input.GetMouseButton(1)) // right mouse button
        {
            accelerometerY = -1; // backward
        }

        // Apply torque to rotate the car
        rb.AddTorque(new Vector3(0, accelerometerX * rotationSpeed, 0), ForceMode.VelocityChange);

        // Apply force to move the car
        rb.AddForce(new Vector3(accelerometerY * speed, 0, 0), ForceMode.Acceleration);

        // Update previous mouse position
        mouseXPrev = mousePosition.x;
        mouseYPrev = mousePosition.y;
    }
}