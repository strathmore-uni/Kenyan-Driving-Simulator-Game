using UnityEngine;

public class CarGearSystem : MonoBehaviour
{
    public enum Gear { Park, Reverse, Neutral, Drive }
    public Gear currentGear = Gear.Park;

    private Rigidbody carRigidbody;
    public float driveForce = 1500f;
    public float reverseForce = 1000f;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        switch (currentGear)
        {
            case Gear.Park:
                // Stop the car immediately and prevent further movement
                StopCar();
                break;
            case Gear.Neutral:
                // Allow the car to coast, but don't apply any force
                break;
            case Gear.Drive:
                // Apply force for forward movement (based on user input)
                ApplyDrive();
                break;
            case Gear.Reverse:
                // Apply force for reverse movement (based on user input)
                ApplyReverse();
                break;
        }
    }

    public void SetGear(string gear)
    {
        if (gear == "P")
        {
            currentGear = Gear.Park;
        }
        else if (gear == "N")
        {
            currentGear = Gear.Neutral;
        }
        else if (gear == "D")
        {
            currentGear = Gear.Drive;
        }
        else if (gear == "R")
        {
            currentGear = Gear.Reverse;
        }
    }

    private void StopCar()
    {
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
    }

    private void ApplyDrive()
    {
        float accelerationInput = SimpleInput.GetAxis("Vertical");
        carRigidbody.AddForce(transform.forward * accelerationInput * driveForce);
    }

    private void ApplyReverse()
    {
        float reverseInput = SimpleInput.GetAxis("Vertical");
        carRigidbody.AddForce(-transform.forward * reverseInput * reverseForce);
    }

    // Method to check if the car is in Drive gear
    public bool IsInDrive()
    {
        return currentGear == Gear.Drive;
    }

    // Method to check if the car is in Reverse gear
    public bool IsInReverse()
    {
        return currentGear == Gear.Reverse;
    }
}
