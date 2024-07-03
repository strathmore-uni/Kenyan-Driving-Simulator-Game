using UnityEngine;

public class CamControl : MonoBehaviour
{
    public GameObject rearCamera;
    public GameObject frontCamera;
    public GameObject topDownCamera;

    private Vector2 startPos;
    private bool fingerDown = false;
    private float pixelDistToDetect = 50f; // adjust this value to your liking

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            fingerDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (fingerDown)
            {
                Vector2 endPos = Input.mousePosition;
                float deltaX = endPos.x - startPos.x;
                float deltaY = endPos.y - startPos.y;

                if (Mathf.Abs(deltaX) > pixelDistToDetect)
                {
                    if (deltaX > 0)
                    {
                        SwitchToFrontCamera();
                    }
                    else
                    {
                        SwitchToRearCamera();
                    }
                }
                else if (Mathf.Abs(deltaY) > pixelDistToDetect)
                {
                    if (deltaY > 0)
                    {
                        SwitchToTopDownCamera();
                    }
                }

                fingerDown = false;
            }
        }
    }

    void SwitchToRearCamera()
    {
        rearCamera.SetActive(true);
        frontCamera.SetActive(false);
        topDownCamera.SetActive(false);
    }

    void SwitchToFrontCamera()
    {
        rearCamera.SetActive(false);
        frontCamera.SetActive(true);
        topDownCamera.SetActive(false);
    }

    void SwitchToTopDownCamera()
    {
        rearCamera.SetActive(false);
        frontCamera.SetActive(false);
        topDownCamera.SetActive(true);
    }
}