using UnityEngine;

public class InsideBlinkerRenderer : MonoBehaviour
{
    // References to the outside blinker lights
    public Light leftBlinkerLight;
    public Light rightBlinkerLight;

    // References to the inside blinker renderers
    public Renderer leftInsideBlinkerRenderer;
    public Renderer rightInsideBlinkerRenderer;

    private void Update()
    {
        // Update the inside blinker renderers based on the outside blinker lights
        leftInsideBlinkerRenderer.enabled = leftBlinkerLight.enabled;
        rightInsideBlinkerRenderer.enabled = rightBlinkerLight.enabled;
    }
}