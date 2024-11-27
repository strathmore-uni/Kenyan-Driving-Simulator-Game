using UnityEngine;

public class SteeringWheelController : MonoBehaviour
{
    public Transform steeringWheel;
    public float maxSteerAngle = 30f;
    public float smoothFactor = 5f;

    void Update()
    {
        float steerInput = SimpleInput.GetAxis("Horizontal"); // Or touch input for mobile
        float targetAngle = steerInput * maxSteerAngle;

        // Smoothly rotate the steering wheel
        float currentAngle = Mathf.LerpAngle(steeringWheel.localEulerAngles.z, targetAngle, Time.deltaTime * smoothFactor);
        steeringWheel.localEulerAngles = new Vector3(0, 0, currentAngle);
    }
}
