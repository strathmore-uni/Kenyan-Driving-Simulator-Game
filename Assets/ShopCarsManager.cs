using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCarsManager : MonoBehaviour
{
    public int currentCarIndex = 0;
    public GameObject[] carModels;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject car in carModels)
            car.SetActive(false);

        carModels[currentCarIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
