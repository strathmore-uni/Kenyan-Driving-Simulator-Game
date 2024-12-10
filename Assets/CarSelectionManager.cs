using UnityEngine;

public class CarSelectionManager : MonoBehaviour
{
    public static CarSelectionManager Instance; // Singleton instance

    public GameObject[] cars; // Array to hold all car prefabs
    public int selectedCarIndex = 0; // Index of the selected car

    private void Awake()
    {
        // Ensure this object persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Set the selected car
    public void SelectCar(int carIndex)
    {
        selectedCarIndex = carIndex;
    }
}
