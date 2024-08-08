using UnityEngine;

public class CarController : MonoBehaviour
{
    // Movement variables
    public float speed = 10.0f;
    public float turnSpeed = 5.0f;

    // Reference to the Rigidbody component
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get accelerometer data
        float accelerometerX = Input.acceleration.x;
        float accelerometerY = Input.acceleration.y;

        // Move the car
        Vector3 movementDirection = new Vector3(accelerometerX, 0, accelerometerY);
        rb.AddForce(movementDirection * speed, ForceMode.Acceleration);

        // Get gyroscope data
        float gyroscopeZ = Input.gyro.attitude.eulerAngles.z;

        // Turn the car
        float turnDirection = gyroscopeZ * turnSpeed;
        rb.AddTorque(transform.up * turnDirection, ForceMode.Acceleration);
    }
}