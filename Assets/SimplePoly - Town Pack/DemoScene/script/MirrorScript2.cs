using UnityEngine;

public class MirrorScript2 : MonoBehaviour
{
    // The camera that will capture the reflection
    public Camera mirrorCamera;

    // The render texture that will store the reflection
    public RenderTexture mirrorTexture;

    // The material that will display the reflection
    public Material mirrorMaterial;

    // The GameObject that will display the reflection
    public GameObject mirrorObject;

    // The layer mask that will be used to cull objects from the reflection
    public LayerMask cullMask;

    private void Start()
    {
        // Create a new render texture if one doesn't exist
        if (!mirrorTexture)
        {
            mirrorTexture = new RenderTexture(512, 512, 16);
        }

        // Set the render texture as the target texture for the mirror camera
        mirrorCamera.targetTexture = mirrorTexture;

        // Set the cull mask for the mirror camera
        mirrorCamera.cullingMask = cullMask;

        // Set the material to display the reflection
        mirrorObject.GetComponent<Renderer>().material = mirrorMaterial;
    }

    private void LateUpdate()
    {
        // Render the reflection
        mirrorCamera.Render();

        // Set the reflection texture as the main texture for the material
        mirrorMaterial.mainTexture = mirrorTexture;
    }
}