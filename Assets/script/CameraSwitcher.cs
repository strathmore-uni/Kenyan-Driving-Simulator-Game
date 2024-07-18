using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera backViewCamera;
    public Camera frontViewCamera;
    public Camera leftViewCamera;
    public Camera rightViewCamera;
    public Camera interiorViewCamera;
    public Canvas canvas;

    public Button interiorViewButton;

    private Camera currentCamera;

    void Start()
    {
        currentCamera = backViewCamera;
        canvas.enabled = true;

        interiorViewButton.onClick.AddListener(SwitchToInteriorView);
    }

    public void SwitchToInteriorView()
    {
        SwitchToCamera(interiorViewCamera);
        canvas.enabled = false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                fingerDown = true;
            }
            else if (touch.phase == TouchPhase.Ended && fingerDown)
            {
                fingerDown = false;
                float swipeDistance = touch.position.x - startPos.x;
                if (swipeDistance > pixelDistToDetect)
                {
                    SwitchToCamera(rightViewCamera);
                    canvas.enabled = true;
                }
                else if (swipeDistance < -pixelDistToDetect)
                {
                    SwitchToCamera(leftViewCamera);
                    canvas.enabled = true;
                }
                else if (touch.position.y - startPos.y > pixelDistToDetect)
                {
                    SwitchToCamera(frontViewCamera);
                    canvas.enabled = true;
                }
                else if (touch.position.y - startPos.y < -pixelDistToDetect)
                {
                    SwitchToCamera(backViewCamera);
                    canvas.enabled = true;
                }
            }
        }
    }

    void SwitchToCamera(Camera camera)
    {
        if (currentCamera != null)
        {
            currentCamera.enabled = false;
        }
        camera.enabled = true;
        currentCamera = camera;
    }

    private Vector2 startPos;
    private bool fingerDown = false;
    private float pixelDistToDetect = 50f;
}