using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParkingCollisionCheck : MonoBehaviour
{
    public ParkingCar car;      // Reference to the ParkingCar script
    public ParkingLot lot;      // Reference to the ParkingLot script
    public Text countdownText;  // UI Text element to display countdown
    public float countdownTime = 3.0f; // Time in seconds to wait before showing the passed panel
    private float countdownTimer;
    private bool isCountdownActive = false;

    public bool enableDebugVisualization = true; // Toggle for debug visualization

    void Start()
    {
        // Ensure FailedPanel and PassedPanel are inactive at the start
        car.ShowFailedPanel(false);
        car.ShowPassPanel(false);

        // Initialize the countdown timer
        countdownTimer = countdownTime;
        countdownText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Check for collision between car and parking lot boundaries
        bool collision = car.CheckCollision(lot);

        if (collision)
        {
            Debug.Log("Collision detected with parking lot boundaries.");
            car.ShowPassPanel(false); // Hide the pass panel on collision
            ResetCountdown(); // Reset the countdown if collision happens
        }
        else
        {
            Debug.Log("No collision detected. Car is within parking lot boundaries.");
            if (!isCountdownActive)
            {
                StartCountdown();
            }
        }

        if (enableDebugVisualization)
        {
            VisualizeCollision(car, lot);
        }

        // Update the countdown timer if it's active
        if (isCountdownActive)
        {
            countdownTimer -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(countdownTimer).ToString();

            if (countdownTimer <= 0)
            {
                car.ShowPassPanel(true);
                countdownText.gameObject.SetActive(false);
                isCountdownActive = false; // Stop the countdown
            }
        }
    }

    void StartCountdown()
    {
        countdownTimer = countdownTime;
        countdownText.gameObject.SetActive(true);
        isCountdownActive = true;
    }

    void ResetCountdown()
    {
        countdownTimer = countdownTime;
        countdownText.gameObject.SetActive(false);
        isCountdownActive = false;
    }

    void VisualizeCollision(ParkingCar car, ParkingLot lot)
    {
        // Calculate half width and length vectors based on car dimensions
        var halfWidthVector = car.transform.right * car.width * 0.5f;
        var halfLengthVector = car.transform.forward * car.length * 0.5f;

        // Determine the corners of the car
        var a = car.transform.position + halfWidthVector + halfLengthVector;
        var b = car.transform.position - halfWidthVector + halfLengthVector;
        var c = car.transform.position + halfWidthVector - halfLengthVector;
        var d = car.transform.position - halfWidthVector - halfLengthVector;

        // Visualize the car's AABB
        Debug.DrawLine(a, b, Color.red);
        Debug.DrawLine(b, d, Color.red);
        Debug.DrawLine(d, c, Color.red);
        Debug.DrawLine(c, a, Color.red);

        // Visualize the parking lot bounds
        Vector3 lotMin = lot.transform.position - new Vector3(lot.width * 0.5f, 0, lot.length * 0.5f);
        Vector3 lotMax = lot.transform.position + new Vector3(lot.width * 0.5f, 0, lot.length * 0.5f);

        Debug.DrawLine(lotMin, lotMin + new Vector3(lot.width, 0, 0), Color.blue);
        Debug.DrawLine(lotMin, lotMin + new Vector3(0, 0, lot.length), Color.blue);
        Debug.DrawLine(lotMax, lotMax - new Vector3(lot.width, 0, 0), Color.blue);
        Debug.DrawLine(lotMax, lotMax - new Vector3(0, 0, lot.length), Color.blue);
    }

    private void OnCollisionEnter(Collision human)
    {
        // Check if the car collides with a game object tagged "Human"
        if (human.collider.CompareTag("Human"))
        {
            car.ShowFailedPanel(true);
            ResetCountdown(); // Reset the countdown if collision happens
        }
    }
}
