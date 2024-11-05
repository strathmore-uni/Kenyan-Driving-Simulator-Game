using UnityEngine;

public class ReverseGearScript : MonoBehaviour
{
    public enum Gear { Park, Neutral, Drive, Reverse }
    public Gear currentGear = Gear.Neutral;

    public float reverseSpeed = 10f; // Speed for reversing
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SwitchGear(Gear newGear)
    {
        currentGear = newGear;
        Debug.Log("Switched to: " + currentGear);
    }

    public void MoveCar(float gasInput)
    {
        if (currentGear == Gear.Reverse)
        {
            Vector3 moveDirection = -transform.forward * gasInput * reverseSpeed; // Move backwards
            rb.AddForce(moveDirection);
        }
    }

    // Call this method when the reverse gear button is pressed
    public void OnSwitchToReverse()
    {
        SwitchGear(Gear.Reverse);
    }

    // Call this method when the gas pedal is pressed
    public void OnGasPedalPressed()
    {
        if (currentGear == Gear.Reverse)
        {
            MoveCar(1f); // Full gas
        }
    }

    // Call this method when the gas pedal is released
    public void OnGasPedalReleased()
    {
        MoveCar(0f); // No gas
    }
}
