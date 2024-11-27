using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public GameObject[] Cars;
    int ram = 0;

    private void Start()
    {
        Spawn();
    }
    void Spawn()
    {
        for(int i=0; i < transform.childCount;i++)
        {
            ram = Random.Range(0, Cars.Length);
            Instantiate(Cars[ram], transform.GetChild(i).transform.position, transform.GetChild(i).transform.rotation);

        }
    }
}
