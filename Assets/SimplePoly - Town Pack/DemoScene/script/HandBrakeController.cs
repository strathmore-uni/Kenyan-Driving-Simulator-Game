using UnityEngine;

public class CarHandbrake : MonoBehaviour
{
    [Header("Handbrake Settings")]
    public Rigidbody carRigidbody; // The car's Rigidbody
    public float brakeForce = 1000f; // Force to apply to stop the car
    public GameObject handbrakeButton; // UI Button for the handbrake

    private bool isHandbrakeEngaged = false;

    private void Start()
    {
        if (carRigidbody == null)
        {
            Debug.LogError("Car Rigidbody is not assigned!");
            return;
        }

        if (handbrakeButton != null)
        {
            // Add a listener to the handbrake button
            handbrakeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ToggleHandbrake);
        }
        else
        {
            Debug.LogError("Handbrake button is not assigned!");
        }
    }

    public void ToggleHandbrake()
    {
        // Toggle the handbrake state
        isHandbrakeEngaged = !isHandbrakeEngaged;

        if (isHandbrakeEngaged)
        {
            ApplyHandbrake();
        }
        else
        {
            ReleaseHandbrake();
        }
    }

    private void ApplyHandbrake()
    {
        // Stop the car's velocity and angular velocity
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;

        // Optionally, increase drag to prevent further movement
        carRigidbody.drag = 10f;
        carRigidbody.angularDrag = 10f;

        Debug.Log("Handbrake Applied");
    }

    private void ReleaseHandbrake()
    {
        // Restore the original drag values
        carRigidbody.drag = 0f;
        carRigidbody.angularDrag = 0.05f;

        Debug.Log("Handbrake Released");
    }
}
