using UnityEngine;
using UnityEngine.UI;
public class HandbrakeController : MonoBehaviour
{
    public float handbrakeForce = 1000f; // Adjust this value to your liking
    public Button handbrakeButton; // Assign the HandbrakeButton in the Inspector
    private bool isHandbrakeActive = false;
    private Rigidbody rb;
    private float originalDrag;
    private float originalAngularDrag;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
        originalAngularDrag = rb.angularDrag;
        handbrakeButton.onClick.AddListener(HandleHandbrakeClick);
    }

    void HandleHandbrakeClick()
    {
        isHandbrakeActive = !isHandbrakeActive;
        if (isHandbrakeActive)
        {
            ApplyHandbrake();
        }
        else
        {
            ReleaseHandbrake();
        }
    }

    void ApplyHandbrake()
    {
        // Increase drag to slow down the car
        rb.drag = 10f;
        rb.angularDrag = 10f;

        // Apply a force to the rear wheels to simulate the handbrake
        Vector3 handbrakeForceVector = transform.forward * handbrakeForce;
        rb.AddForceAtPosition(handbrakeForceVector, transform.position + transform.forward * 2f);
    }

    void ReleaseHandbrake()
    {
        // Reset drag and angular drag
        rb.drag = originalDrag;
        rb.angularDrag = originalAngularDrag;
    }
}