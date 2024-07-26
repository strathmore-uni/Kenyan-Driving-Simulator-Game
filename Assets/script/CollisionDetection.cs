using UnityEngine;

public class VehicleCollisionSound : MonoBehaviour
{
    public AudioClip collisionSound; // Audio clip played on collision
    public float collisionForceThreshold = 10f; // Minimum force required to trigger sound

    private AudioSource audioSource; // Reference to the AudioSource component
    private Rigidbody rb; // Reference to the Rigidbody component

    void Start()
    {
        // Add an AudioSource component to the vehicle
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Don't play the sound on awake

        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        // Update the position of the AudioSource component
        audioSource.transform.position = rb.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision force is above the threshold
        float collisionForce = collision.impulse.magnitude;
        if (collisionForce > collisionForceThreshold)
        {
            // Play the collision sound
            audioSource.PlayOneShot(collisionSound);

            // Debug logging
            Debug.Log("Collision with another object!");
        }
    }
}