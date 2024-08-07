using UnityEngine;

public class InteriorSteeringController : MonoBehaviour
{
    [SerializeField]
    public Transform steeringWheel;
    public float steeringSensitivity = 10f;
    public float maxSteeringAngle = 30f;

    private float initialRotation;
    private bool isSteering;
    public float rotationSpeed = 100.0f;

    private float rotation = 0.0f;

    void Start()
    {
        initialRotation = steeringWheel.localEulerAngles.y;
    }

    void Update()
    {
        // Check for touch input on mobile devices
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                // Start steering when the user touches the steering wheel
                isSteering = true;
                Debug.Log("Steering started");
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Update steering angle based on touch movement
                float deltaX = touch.deltaPosition.x * 0.01f; // Reduce the sensitivity
                float targetRotation = initialRotation + deltaX;
                targetRotation = Mathf.Clamp(targetRotation, initialRotation - maxSteeringAngle, initialRotation + maxSteeringAngle);
                steeringWheel.localEulerAngles = new Vector3(0, targetRotation, 0);
                Debug.Log("Steering angle: " + targetRotation);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Stop steering when the user releases the touch
                isSteering = false;
                Debug.Log("Steering ended");
            }
        }
        float horizontalInput = SimpleInput.GetAxis("Horizontal");

        rotation += horizontalInput * rotationSpeed * Time.deltaTime;
        steeringWheel.localEulerAngles = new Vector3(0, rotation, 0);
    }
 
}