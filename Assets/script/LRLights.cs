using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRLights : MonoBehaviour
{
    public GameObject lrLight;
    float baseDuration = 0.5f;
    float countDown = .5f;
    bool isOn = false;
    
    // Start is called before the first frame update
    void Start()
    {
        countDown = baseDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown>0){
            countDown -= Time.deltaTime;
        }
        else{
            countDown = baseDuration;
            if(isOn){
                isOn = false;
                lrLight.SetActive(false);
            }
            else{
                isOn = true;
                lrLight.SetActive(true);
            }
        }
    }

    public void ResetTimer(){
        countDown = baseDuration;
        isOn = true;
        lrLight.SetActive(true);
    }
}
