using UnityEngine;

public class SideMirrorController : MonoBehaviour
{
    // Reference to the side mirror GameObjects
    public GameObject leftSideMirror;
    public GameObject rightSideMirror;

    // Reference to the Reflection Probes
    public ReflectionProbe leftReflectionProbe;
    public ReflectionProbe rightReflectionProbe;

    // Rotation speed for the side mirrors
    public float rotationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the side mirrors based on user input
        RotateSideMirrors();
    }

    // Rotate the side mirrors based on user input
    private void RotateSideMirrors()
    {
        // Get the horizontal input from the user
        float horizontalInput = Input.GetAxis("Horizontal");

        // Rotate the side mirrors
        leftSideMirror.transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        rightSideMirror.transform.Rotate(Vector3.up, -horizontalInput * rotationSpeed * Time.deltaTime);

        // Update the Reflection Probes
        leftReflectionProbe.transform.position = leftSideMirror.transform.position;
        rightReflectionProbe.transform.position = rightSideMirror.transform.position;
    }
}