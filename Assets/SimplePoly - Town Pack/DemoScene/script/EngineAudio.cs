using MyNamespace;
using System.Collections;
using UnityEngine;

public class EngineAudio : MonoBehaviour
{
    public AudioSource runningSound;
    public float runningMaxVolume = 1f;
    public float runningMaxPitch = 1f;
    public AudioSource reverseSound;
    public float reverseMaxVolume = 1f;
    public float reverseMaxPitch = 1f;
    public AudioSource idleSound;
    public float idleMaxVolume = 1f;
    public float speedRatio;
    private float revLimiter;
    public float LimiterSound = 1f;
    public float LimiterFrequency = 3f;
    public float LimiterEngage = 0.8f;
    public bool isEngineRunning = false;
    public AudioSource startingSound;

    private CarManager carManager;

    // Start is called before the first frame update
    void Start()
    {
        carManager = GetComponent<CarManager>();
        idleSound.volume = 0;
        runningSound.volume = 0;
        reverseSound.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float speedSign = 0;
        if (carManager)
        {
            speedSign = Mathf.Sign(carManager.GetSpeedRatio());
            speedRatio = Mathf.Abs(carManager.GetSpeedRatio());
        }

        if (speedRatio > LimiterEngage)
        {
            revLimiter = (Mathf.Sin(Time.time * LimiterFrequency) + 1f) * LimiterSound * (speedRatio - LimiterEngage);
        }

        if (isEngineRunning)
        {
            idleSound.volume = Mathf.Lerp(0.1f, idleMaxVolume, speedRatio);

            if (speedSign > 0)
            {
                reverseSound.volume = 0;
                runningSound.volume = Mathf.Lerp(0.3f, runningMaxVolume, speedRatio + revLimiter);
                runningSound.pitch = Mathf.Lerp(0.3f, runningMaxPitch, speedRatio);
            }
            else
            {
                runningSound.volume = 0;
                reverseSound.volume = Mathf.Lerp(0f, reverseMaxVolume, speedRatio + revLimiter);
                reverseSound.pitch = Mathf.Lerp(0.2f, reverseMaxPitch, speedRatio);
            }
        }
        else
        {
            idleSound.volume = 0;
            runningSound.volume = 0;
            reverseSound.volume = 0;
        }
    }

    public IEnumerator StartEngine()
    {
        startingSound.Play();
        carManager.isEngineRunning = 1;  // Use the carManager reference
        yield return new WaitForSeconds(0.6f);
        isEngineRunning = true;
        yield return new WaitForSeconds(0.4f);
        carManager.isEngineRunning = 2;  // Use the carManager reference
    }
}
