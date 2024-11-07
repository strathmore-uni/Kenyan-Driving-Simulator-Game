using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Background music and sound effects audio sources
    public AudioSource bgMusicSource;
    public AudioSource carEngineAudioSource;

    private void Awake()
    {
        // Ensure there's only one instance of AudioManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Set background music volume
    public void SetBGMusicVolume(float volume)
    {
        bgMusicSource.volume = volume;
    }

    // Set sound effects volume (like car engine)
    public void SetSoundEffectsVolume(float volume)
    {
        carEngineAudioSource.volume = volume;
    }

    // Mute/unmute background music
    public void MuteBGMusic(bool mute)
    {
        bgMusicSource.mute = mute;
    }

    // Mute/unmute sound effects
    public void MuteSoundEffects(bool mute)
    {
        carEngineAudioSource.mute = mute;
    }
}
