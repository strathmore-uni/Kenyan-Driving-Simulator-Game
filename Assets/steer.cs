using MyNamespace;
using UnityEngine;

public class SimpleSteeringTest : MonoBehaviour
{
    public GameObject car; // Reference to the car
    public float maxSteerAngle = 30f; // Maximum angle the car can steer
    public float steerInput = 0f; // Test input value

    void Update()
    {
        // Test steering with a fixed input value
        car.GetComponent<CarManager>().Steer(steerInput * maxSteerAngle);

        // Optionally update the steering wheel's rotation for visual feedback
        Transform steeringWheel = transform; // Assuming this script is attached to the steering wheel
        steeringWheel.localRotation = Quaternion.Euler(0, 0, -steerInput * maxSteerAngle);
    }
}
