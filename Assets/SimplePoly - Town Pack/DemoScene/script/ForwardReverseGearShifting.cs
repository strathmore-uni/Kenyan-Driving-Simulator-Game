using UnityEngine;
using UnityEngine.UI;

public class ForwardReverseGearShifting : MonoBehaviour
{
    public enum Gear { Forward, Reverse }
    public Gear currentGear = Gear.Forward;

    public Button gearShiftButton; // Assign this in the Inspector
    private Text gearText; // Assign this in the Inspector

    void Start()
    {
        gearShiftButton.onClick.AddListener(ShiftGear);
        UpdateGearText();
    }

    void ShiftGear()
    {
        if (currentGear == Gear.Forward)
        {
            currentGear = Gear.Reverse;
        }
        else
        {
            currentGear = Gear.Forward;
        }
        UpdateGearText();
    }

    void UpdateGearText()
    {
        gearText.text = currentGear == Gear.Forward ? "Forward" : "Reverse";
    }

    // Call this function to apply the gear direction to the car's motor torque
    public float GetGearDirection()
    {
        return currentGear == Gear.Forward ? 1f : -1f;
    }
}