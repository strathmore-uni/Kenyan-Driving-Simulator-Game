using UnityEngine;

public class Car1InteriorSteering : MonoBehaviour
{
    public WheelCollider ILWheelCollider, IRWheelCollider; // Interior left and right wheel colliders
    public float interiorSteeringSensitivity = 1.0f; // Adjust this value to match your exterior steering sensitivity

    void Update()
    {
        // Get the steering input (same as exterior steering)
        float steeringInput = SimpleInput.GetAxis("Horizontal");

        // Apply the interior steering sensitivity factor
        steeringInput *= interiorSteeringSensitivity;
        steeringInput = Mathf.Clamp(steeringInput, -1f, 1f); // Clamp the input

        // Calculate the interior steering angle (same as exterior steering)
        float interiorSteeringAngle = steeringInput * 30f; // Assuming max interior steering angle is 30 degrees

        // Apply the interior steering angle to the interior wheel colliders
        ILWheelCollider.steerAngle = interiorSteeringAngle;
        IRWheelCollider.steerAngle = interiorSteeringAngle;

        // Debug logs to check values
        Debug.Log($"Interior Steering Input: {steeringInput}, Interior Steering Angle: {interiorSteeringAngle}");
    }

    // You can also add a separate ApplySteering() method for interior steering, similar to the exterior steering script
}