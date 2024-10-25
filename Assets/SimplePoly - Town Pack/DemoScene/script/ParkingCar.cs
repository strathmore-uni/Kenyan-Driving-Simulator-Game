using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingCar : MonoBehaviour
{
    public float width = 2.0f;  // Width of the car
    public float length = 4.0f; // Length of the car

    public static ParkingCar Instance;
    public GameObject PassedPanel;
    public GameObject FailedPanel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of ParkingCar detected!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Ensure panels are inactive at the start
        PassedPanel.SetActive(false);
        FailedPanel.SetActive(false);
    }

    public void ShowPassPanel(bool show)
    {
        PassedPanel.SetActive(show);
    }

    public void ShowFailedPanel(bool show)
    {
        FailedPanel.SetActive(show);
    }
    // Add this method to your ParkingCar script
    public void ResetToDrivingMode()
    {
        ShowPassPanel(false);
        // Reset any other necessary car parameters, like speed or state
    }

    public void OnClosePassPanelButtonClick()
    {
        ResetToDrivingMode();
    }

    public bool CheckCollision(ParkingLot lot)
    {
        // Recalculate car position in case it moves
        Vector3 carPos = transform.position;

        // Calculate half width and length vectors based on car dimensions
        var halfWidthVector = transform.right * width * 0.5f;
        var halfLengthVector = transform.forward * length * 0.5f;

        // Determine the corners of the car
        var a = carPos + halfWidthVector + halfLengthVector;
        var b = carPos - halfWidthVector + halfLengthVector;
        var c = carPos + halfWidthVector - halfLengthVector;
        var d = carPos - halfWidthVector - halfLengthVector;

        // Determine the minimum and maximum AABB (Axis-Aligned Bounding Box) coordinates
        var aabbMin = Vector3.Min(Vector3.Min(a, b), Vector3.Min(c, d));
        var aabbMax = Vector3.Max(Vector3.Max(a, b), Vector3.Max(c, d));

        // Perform overlap test with parking lot bounds
        bool noOverlap =
            (aabbMin.x >= lot.transform.position.x - lot.width * 0.5f)
            && (aabbMax.x <= lot.transform.position.x + lot.width * 0.5f)
            && (aabbMin.z >= lot.transform.position.z - lot.length * 0.5f)
            && (aabbMax.z <= lot.transform.position.z + lot.length * 0.5f);

        // Return whether there is a collision (i.e., the car is out of bounds)
        return !noOverlap;
    }
}
