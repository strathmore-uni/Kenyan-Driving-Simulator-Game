using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    public AudioSource audioSource; // Make AudioSource public
    public AudioClip collisionClip; // Add a public AudioClip field

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Assign the audio clip to the audio source
        if (collisionClip != null)
        {
            audioSource.clip = collisionClip;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.relativeVelocity.magnitude > 2)
            audioSource.Play();
    }
}
