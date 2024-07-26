using UnityEngine;

public class BrakeReverseLights : MonoBehaviour
{
	public GameObject brakeLight;
	public GameObject reverseLight;
	public Rigidbody vehicleRigidbody;

	private float brakeThreshold = 1.0f; // Speed threshold to consider the vehicle braking
	private float reverseThreshold = -1.0f; // Speed threshold to consider the vehicle reversing

    void Start()
    {
    }
	void Update()
	{
		float speed = vehicleRigidbody.velocity.magnitude;
		float forwardSpeed = Vector3.Dot(vehicleRigidbody.velocity, transform.forward);

		if (forwardSpeed < brakeThreshold && forwardSpeed > reverseThreshold)
		{
			TurnOnBrakeLight();
			TurnOffReverseLight();
		}
		else if (forwardSpeed <= reverseThreshold)
		{
			TurnOffBrakeLight();
			TurnOnReverseLight();
		}
		else
		{
			TurnOffBrakeLight();
			TurnOffReverseLight();
		}
	}

	private void TurnOnBrakeLight()
	{
		brakeLight.SetActive(true);
		foreach (Transform child in brakeLight.transform)
		{
			child.gameObject.SetActive(true);
		}
	}

	private void TurnOffBrakeLight()
	{
		brakeLight.SetActive(false);
		foreach (Transform child in brakeLight.transform)
		{
			child.gameObject.SetActive(false);
		}
	}

	private void TurnOnReverseLight()
	{
		reverseLight.SetActive(true);
		foreach (Transform child in reverseLight.transform)
		{
			child.gameObject.SetActive(true);
		}
	}

	private void TurnOffReverseLight()
	{
		reverseLight.SetActive(false);
		foreach (Transform child in reverseLight.transform)
		{
			child.gameObject.SetActive(false);
		}
	}
}