using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public Transform spawnPoint; // Position where the car will spawn
    public GameObject defaultCar; // Assign this in the Inspector

    private void Start()
    {
        int selectedCarIndex = CarSelectionManager.Instance.selectedCarIndex;
        GameObject selectedCar = CarSelectionManager.Instance.cars[selectedCarIndex];

        // Destroy or disable the default car
        if (defaultCar != null)
        {
            Destroy(defaultCar); // Or use defaultCar.SetActive(false);
        }

        // Instantiate the selected car
        Instantiate(selectedCar, spawnPoint.position, spawnPoint.rotation);
    }

   
}
