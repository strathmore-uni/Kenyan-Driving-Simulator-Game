using UnityEngine;

public class CarCollisionHandler : MonoBehaviour
{
    public float crashForceThreshold = 10f; // Threshold for detecting a crash
    public AudioClip crashSound; // Sound to play on crash
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision force exceeds the threshold
        if (collision.relativeVelocity.magnitude > crashForceThreshold)
        {
            HandleCrash(collision);
        }
    }

    void HandleCrash(Collision collision)
    {
        // Play crash sound
        if (crashSound != null)
        {
            audioSource.PlayOneShot(crashSound);
        }

        // Apply damage or other crash effects here
        Debug.Log("Crash detected with force: " + collision.relativeVelocity.magnitude);

        // Example: Apply damage to the car
        // CarHealth carHealth = GetComponent<CarHealth>();
        // if (carHealth != null)
        // {
        //     carHealth.ApplyDamage(collision.relativeVelocity.magnitude);
        // }
    }
}