using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum GearStateTwo
{
    Park,   // P
    Reverse, // R
    Neutral, // N
    Drive    // D
}
public class GearManager : MonoBehaviour
{
    public GearStateTwo currentGear2 = GearStateTwo.Park; // Default gear
    public UnityEvent<GearStateTwo> onGearChanged;
    public TMP_Text gearText;
    

    // Call this method to shift gears
    void Start()
    {
        // Initialize the car in Park
        ShiftGear(GearStateTwo.Park);
    }
    void ChangeGear(GearStateTwo newGear)
    {
        currentGear2 = newGear;
    }

    public void ShiftGear(GearStateTwo newGear)
    {
        currentGear2 = newGear;

        // Update the UI for the gear display
        switch (currentGear2)
        {
            case GearStateTwo.Park:
                gearText.text = "P";
                gearText.text = "P";
                break;
            case GearStateTwo.Reverse:
                gearText.text = "R";
                gearText.text = "R";
                break;
            case GearStateTwo.Neutral:
                gearText.text = "N";
                gearText.text = "N";
                break;
            case GearStateTwo.Drive:
                gearText.text = "D";
                gearText.text = "D";
                break;
        }
    }


    public void HandleGearChange()
    {
        switch (currentGear2)
        {
            case GearStateTwo.Park:
                // Logic for Park gear
                Debug.Log("Gear: Park");
                break;
            case GearStateTwo.Reverse:
                // Logic for Reverse gear
                Debug.Log("Gear: Reverse");
                break;
            case GearStateTwo.Neutral:
                // Logic for Neutral gear
                Debug.Log("Gear: Neutral");
                break;
            case GearStateTwo.Drive:
                // Logic for Drive gear
                Debug.Log("Gear: Drive");
                break;
        }
    }
}
