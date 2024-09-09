using UnityEngine;

public class UpdateReflectionProbe : MonoBehaviour
{
    // Reference to the Reflection Probe
    public ReflectionProbe reflectionProbe;

    // Update is called once per frame
    void Update()
    {
        // Update the Reflection Probe position to match the car's position
        reflectionProbe.transform.position = transform.position;

        // Render the Reflection Probe
        reflectionProbe.RenderProbe();
    }
}