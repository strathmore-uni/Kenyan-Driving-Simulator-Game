using UnityEngine;
using UnityEngine.UI;

public class GearSwitcher : MonoBehaviour
{
    public Rigidbody carRigidbody; // Assign your car's Rigidbody here
    public Slider gearSlider; // Reference to the slider

    public int currentGear;   // To keep track of the current gear

    // Define the gear states for clarity
    private enum Gear
    {
        Park = 0,
        Neutral = 1,
        Reverse = 2,
        Drive = 3
    }

    void Start()
    {
        currentGear = (int)Gear.Park; // Initialize to Park
        gearSlider.value = currentGear; // Set initial slider value
        gearSlider.onValueChanged.AddListener(UpdateGear); // Add listener for slider changes
    }

    public void UpdateGear(float gearValue)
    {
        currentGear = Mathf.RoundToInt(gearValue); // Update current gear based on slider value
        Debug.Log("Current Gear: " + (Gear)currentGear);

        // If Park is selected, stop the car immediately
        if ((Gear)currentGear == Gear.Park)
        {
            carRigidbody.velocity = Vector3.zero; // Stop movement
            carRigidbody.angularVelocity = Vector3.zero; // Stop rotation
        }
    }

    void FixedUpdate()
    {
        float gasInput = SimpleInput.GetAxis("Vertical");
        ApplyGas(gasInput);
    }

    public void ApplyGas(float gasInput)
    {
        Debug.Log("Gas Input: " + gasInput + " | Current Gear: " + (Gear)currentGear);

        if ((Gear)currentGear == Gear.Park)
        {
            // Ensure the car does not move in Park by setting velocity to zero
            carRigidbody.velocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;
            return; // Exit function, no force is applied
        }
        else if ((Gear)currentGear == Gear.Neutral)
        {
            // Let the car roll in Neutral, do not apply additional force
            return;
        }
        else if ((Gear)currentGear == Gear.Reverse)
        {
            float force = gasInput * 10f; // Adjust reverse force as needed
            carRigidbody.AddForce(-transform.forward * Mathf.Abs(force), ForceMode.Acceleration);
        }
        else if ((Gear)currentGear == Gear.Drive)
        {
            float force = gasInput * 10f; // Adjust drive force as needed
            carRigidbody.AddForce(transform.forward * Mathf.Abs(force), ForceMode.Acceleration);
        }
    }
}
