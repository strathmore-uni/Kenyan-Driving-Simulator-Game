using UnityEngine;
using UnityEngine.Events;

public class ThrottleButton22 : MonoBehaviour
{
    public UnityEvent onClick;  // This will allow you to add listeners just like Unity's Button

    // Call this method when the pedal is clicked (or touched)
    public void OnButtonPress()
    {
        if (onClick != null)
        {
            onClick.Invoke();  // Trigger the event when clicked
        }
    }
}
