using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public Camera interiorCamera;
    public Canvas canvas;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("Touch detected!");
            Touch touch = Input.touches[0];
            Debug.Log("Touch phase: " + touch.phase);
            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("Touch ended, toggling camera...");
                ToggleCamera();
            }
        }
    }

    void ToggleCamera()
    {
        interiorCamera.enabled = !interiorCamera.enabled;
        canvas.enabled = !canvas.enabled;
    }
}