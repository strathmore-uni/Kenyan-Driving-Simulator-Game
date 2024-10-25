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

    void Start()
    {
        // Ensure panels are inactive at the start
        car.ShowFailedPanel(false);
        car.ShowPassPanel(false);
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
            if (!isCountdownActive && countdownTimer > 0) // Prevent starting a new countdown if already completed
            {
                StartCountdown();
            }
        }

       

        // Update the countdown timer if it's active
        if (isCountdownActive)
        {
            countdownTimer -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(countdownTimer).ToString();

            if (countdownTimer <= 0)
            {
                car.ShowPassPanel(true); // Show pass panel when countdown completes
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
