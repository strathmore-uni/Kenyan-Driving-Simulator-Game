using UnityEngine;

public class SteeringWheelRotation : MonoBehaviour
{
    public float RotationSpeed = 2.0f; // Adjust the rotation speed

    private void Update()
    {
        Quaternion targetRotation = Quaternion.Euler(0, RotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }
}