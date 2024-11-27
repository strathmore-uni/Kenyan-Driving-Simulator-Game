using UnityEngine;

public class MirrorReflection : MonoBehaviour
{
    public Camera mirrorCamera; // The camera that will render the reflection
    public RenderTexture renderTexture; // The render texture that will hold the mirror's view
    public Material mirrorMaterial; // The material applied to the mirror

    void Start()
    {
        // Check if all required components are assigned
        if (mirrorCamera == null || renderTexture == null || mirrorMaterial == null)
        {
            Debug.LogError("Please assign all required components in the inspector.");
            return;
        }

        // Set the render texture to the mirror camera
        mirrorCamera.targetTexture = renderTexture;

        // Assign the render texture to the mirror material
        // Assuming the mirror material uses the standard shader or similar
        mirrorMaterial.mainTexture = renderTexture;
    }
}
