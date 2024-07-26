using UnityEngine;
using UnityEngine.UI; // Required for using UI elements

public class CameraSwitcher : MonoBehaviour
{
    public Camera thirdPersonCamera; // Assign the third-person camera in the Inspector
    public Camera firstPersonCamera; // Assign the first-person camera in the Inspector
    public Canvas mainCanvas; // Assign the main canvas in the Inspector
    public Button toggleButton; // Assign the button in the Inspector

    private bool isFirstPerson = false;
    private float lastTapTime = 0f;
    private float doubleTapThreshold = 0.3f; // Time threshold to consider as a double-tap

    void Start()
    {
        // Ensure the cameras and canvas are properly initialized
        thirdPersonCamera.gameObject.SetActive(true);
        firstPersonCamera.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);

        // Set up the button click event
        toggleButton.onClick.AddListener(ToggleView);
    }

    void Update()
    {
        // Check for double-tap to switch back to third-person view
        if (isFirstPerson && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                HandleDoubleTap();
            }
        }
    }

    public void ToggleView()
    {
        isFirstPerson = !isFirstPerson;
        if (isFirstPerson)
        {
            // Switch to first-person view
            thirdPersonCamera.gameObject.SetActive(false);
            firstPersonCamera.gameObject.SetActive(true);
            mainCanvas.gameObject.SetActive(false);
        }
        else
        {
            // Switch to third-person view
            thirdPersonCamera.gameObject.SetActive(true);
            firstPersonCamera.gameObject.SetActive(false);
            mainCanvas.gameObject.SetActive(true);
        }
    }

    void HandleDoubleTap()
    {
        float currentTapTime = Time.time;

        if (currentTapTime - lastTapTime < doubleTapThreshold)
        {
            ToggleView(); // Switch back to third-person view
        }

        lastTapTime = currentTapTime;
    }
}
