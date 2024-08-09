using UnityEngine;

public class MirrorScript : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Camera mirrorCamera;

    private void Start()
    {
        // Create a new render texture if it doesn't exist
        if (!renderTexture)
        {
            renderTexture = new RenderTexture(512, 512, 24);
        }

        // Create a new camera if it doesn't exist
        if (!mirrorCamera)
        {
            mirrorCamera = new GameObject("Mirror Camera").AddComponent<Camera>();
            mirrorCamera.transform.parent = transform;
            mirrorCamera.transform.localPosition = Vector3.zero;
            mirrorCamera.transform.localRotation = Quaternion.identity;
        }

        // Set up the mirror camera
        mirrorCamera.targetTexture = renderTexture;
        mirrorCamera.fieldOfView = 60f;
        mirrorCamera.nearClipPlane = 0.1f;
        mirrorCamera.farClipPlane = 100f;
    }

    private void LateUpdate()
    {
        // Render the mirror camera's view to the render texture
        mirrorCamera.Render();
    }
}