using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingLot : MonoBehaviour
{
    public float width = 10.0f;  // Width of the parking lot
    public float length = 20.0f; // Length of the parking lot

    void Start()
    {
        // Initialize parking lot boundaries
        transform.localScale = new Vector3(width, 1, length);
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("CarRearPoint"))
        {
            ParkingCar.Instance.ShowPassPanel(true);
            //Time.timeScale = 0f; // Uncomment if needed
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("CarRearPoint"))
        {
            ParkingCar.Instance.ShowPassPanel(false);
        }
    }
}
