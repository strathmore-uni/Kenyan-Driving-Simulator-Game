using UnityEngine;

public class CarHorn : MonoBehaviour
{
    [Header("Horn Settings")]
    public AudioSource hornSound; // Drag your AudioSource component with the horn sound here

    public void PlayHorn()
    {
        if (hornSound != null && !hornSound.isPlaying)
        {
            hornSound.Play(); // Play the horn sound
        }
    }
}
