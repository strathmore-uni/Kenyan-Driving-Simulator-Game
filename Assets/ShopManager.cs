using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject[] carModels;  // Array of car prefabs in the game
    public int currentCarIndex = 0;

    void Start()
    {
        foreach (GameObject car in carModels)
            car.SetActive(false);

        carModels[currentCarIndex].SetActive(true);
    }

}
