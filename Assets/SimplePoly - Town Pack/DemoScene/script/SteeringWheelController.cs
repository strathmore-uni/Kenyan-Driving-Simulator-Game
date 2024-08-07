using UnityEngine;

public class SteeringWheelControlTouch : MonoBehaviour
{
    [Header("Steering Wheel")]
    public Transform steeringWheel; // Assign the steering wheel object here
    public float maxSteeringAngle = 450f; // Maximum rotation of the steering wheel
    public float rotationSpeed = 10f; // Speed of the rotation

    private float currentSteeringAngle;
    private float lastTouchPosX;
    private bool isTouching = false;

    void Update()
    {
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastTouchPosX = touch.position.x;
                    isTouching = true;
                    break;

                case TouchPhase.Moved:
                    if (isTouching)
                    {
                        // Calculate the touch movement
                        float touchDeltaX = touch.position.x - lastTouchPosX;
                        lastTouchPosX = touch.position.x;

                        // Update the steering angle based on touch movement
                        float steeringDelta = touchDeltaX / Screen.width * maxSteeringAngle;
                        currentSteeringAngle = Mathf.Clamp(currentSteeringAngle + steeringDelta, -maxSteeringAngle, maxSteeringAngle);
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isTouching = false;
                    break;
            }
        }

        // Smoothly rotate the steering wheel towards the current steering angle
        steeringWheel.localRotation = Quaternion.Euler(0f, 0f, -currentSteeringAngle);
    }
}
