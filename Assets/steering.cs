using UnityEngine;

public class Car1Steering : MonoBehaviour
{
    public WheelCollider FLWheelCollider, FRWheelCollider;
    public float steeringSensitivity = 1.0f;

    void Update()
    {
        // Get the steering input
        float steeringInput = SimpleInput.GetAxis("Horizontal");

        // Apply the steering sensitivity factor
        steeringInput *= steeringSensitivity;
        steeringInput = Mathf.Clamp(steeringInput, -1f, 1f); // Clamp the input

        // Calculate the steering angle
        float steeringAngle = steeringInput * 30f; // Assuming max steering angle is 30 degrees



        // Apply the steering angle to the wheel colliders
        FLWheelCollider.steerAngle = steeringAngle;
        FRWheelCollider.steerAngle = steeringAngle;

        // Debug logs to check values
        Debug.Log($"Steering Input: {steeringInput}, Steering Angle: {steeringAngle}");
    }
    void ApplySteering()
    {
        // Get the steering input
        float steeringInput = SimpleInput.GetAxis("Horizontal");

        // Apply the steering sensitivity factor
        steeringInput *= steeringSensitivity;
        steeringInput = Mathf.Clamp(steeringInput, -1f, 1f); // Clamp the input

        // Calculate the steering angle
        float targetSteeringAngle = steeringInput * 30f; // Assuming max steering angle is 30 degrees

        // Smoothly interpolate the current steering angle to the target angle
        float currentSteeringAngle = FLWheelCollider.steerAngle;
        float smoothedSteeringAngle = Mathf.Lerp(currentSteeringAngle, targetSteeringAngle, Time.deltaTime * 5f); // Adjust the smoothing factor

        // Apply the smoothed and adjusted steering angle
        FLWheelCollider.steerAngle = smoothedSteeringAngle;
        FRWheelCollider.steerAngle = smoothedSteeringAngle;

        // Debug logs to check values
        Debug.Log($"Steering Input: {steeringInput}, Steering Angle: {smoothedSteeringAngle}");
    }

}
