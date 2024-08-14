using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for using UI elements

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject interiorCanvas; // Reference to the interior canvas
    public Button interiorButton; // Reference to the button for switching to the interior view
    private bool gameStarted = false; // Track if the game has started

    void Start()
    {
        // Ensure the interior canvas is hidden at the start (in case it's not set in Inspector)
        interiorCanvas.SetActive(false);

        // Keep the interior button active from the start
        interiorButton.gameObject.SetActive(true);

        // Add listener to the button to show the interior canvas when pressed
        interiorButton.onClick.AddListener(ShowInteriorView);
    }

    void Update()
    {
        // Detect touch input to start the game (if necessary)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !gameStarted)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        // Mark the game as started
        gameStarted = true;
    }

    void ShowInteriorView()
    {
        // Only show the interior canvas if the game has started
        if (gameStarted)
        {
            interiorCanvas.SetActive(true); // Show the interior view
        }
    }
}
