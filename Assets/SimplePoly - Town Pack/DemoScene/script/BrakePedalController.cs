using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakePedalController : MonoBehaviour
{
    public WheelCollider FLWheelCollider;  // Front left wheel collider
    public WheelCollider FRWheelCollider;  // Front right wheel collider
    public WheelCollider RLWheelCollider;  // Rear left wheel collider
    public WheelCollider RRWheelCollider;  // Rear right wheel collider

    public float brakingForce = 3000f;      // Braking force applied when brake pedal is pressed
    public float brakeInput;                // Brake input value (0-1)

    public bool isBraking;                  // Flag to check if braking is applied
    public bool brakePedalPressed;          // Simulating brake pedal press (you'll connect this to actual button/input in mobile)
    public bool brakePedalReleased;
    private void Update()
    {
        // Check if brake pedal is pressed
        if (brakePedalPressed)
        {
            Debug.Log("Brake Pedal Pressed"); // Check if the input is detected
            ApplyBrakes();
        }
        else
        {
            ReleaseBrakes();
        }
    }

    // Function to apply brake force
    public void ApplyBrakes()
    {
        isBraking = true;
        brakeInput = 1.0f; // Max braking force

        // Apply braking force to each wheel
        FLWheelCollider.brakeTorque = brakeInput * brakingForce;
        FRWheelCollider.brakeTorque = brakeInput * brakingForce;
        RLWheelCollider.brakeTorque = brakeInput * brakingForce;
        RRWheelCollider.brakeTorque = brakeInput * brakingForce;

        // Debug each wheel to check if force is applied
        Debug.Log("Applying Brakes - Brake Force: " + brakingForce);
        Debug.Log("FL Wheel Brake Torque: " + FLWheelCollider.brakeTorque);
        Debug.Log("FR Wheel Brake Torque: " + FRWheelCollider.brakeTorque);
        Debug.Log("RL Wheel Brake Torque: " + RLWheelCollider.brakeTorque);
        Debug.Log("RR Wheel Brake Torque: " + RRWheelCollider.brakeTorque);
    }

    // Function to release the brakes
    public void ReleaseBrakes()
    {
        isBraking = false;
        brakeInput = 0.0f;

        // Set brake torque to zero for all wheels
        FLWheelCollider.brakeTorque = 0f;
        FRWheelCollider.brakeTorque = 0f;
        RLWheelCollider.brakeTorque = 0f;
        RRWheelCollider.brakeTorque = 0f;

        Debug.Log("Brakes released");
    }

    // Function to simulate pressing brake pedal (for testing)
    public void PressBrakePedal()
    {
        brakePedalPressed = true;
        Debug.Log("Brake pedal pressed");
    }

    // Function to simulate releasing brake pedal (for testing)
    // Function to simulate pressing gas pedal
    public void PressGasPedal()
    {
        // Only release the brakes if they're engaged
        if (isBraking || brakePedalPressed)
        {
            isBraking = false;
            brakePedalPressed = false;
            ReleaseBrakes(); // Release brakes immediately
        }

        // Add logic to apply acceleration here if needed
        Debug.Log("Gas pedal pressed, brakes released");
    }

}
