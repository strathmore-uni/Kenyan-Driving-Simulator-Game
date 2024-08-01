using UnityEngine;

public class StopSignController : MonoBehaviour
{
    public bool isPlayerNearby = false;

    private CarManager carManager;


    private void Start()
    {
        carManager = GameObject.Find("CarManager").GetComponent<CarManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void Update()
    {
        if (isPlayerNearby)
        {
            // Check if the player is stopped at the stop sign
            if (carManager.maxSpeed < 0.1f)
            {
                // The player is stopped, do something
                Debug.Log("Player is stopped at the stop sign");
            }
            else
            {
                // The player is not stopped, do something else
                Debug.Log("Player is not stopped at the stop sign");
            }
        }
    }
}