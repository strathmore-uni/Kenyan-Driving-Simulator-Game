using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLightsController : MonoBehaviour
{
    public GameObject leftIndicator;
    public GameObject rightIndicator;

    public bool leftIndicatorOn = false;
    public bool rightIndicatorOn = false;
    public bool hazardOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnRight(){
        Debug.Log("TurnRight called. Current state: " + rightIndicatorOn);
        if (rightIndicatorOn){
            rightIndicator.SetActive(false); // Set the parent inactive
            foreach (Transform child in rightIndicator.transform) {
                child.gameObject.SetActive(false); // Set each immediate child inactive
            }
            rightIndicatorOn = false;
        }else{
            rightIndicator.SetActive(true); // Set the parent active
        foreach (Transform child in rightIndicator.transform) {
            child.gameObject.SetActive(true); // Set each immediate child active
        }
        leftIndicator.SetActive(false);
        rightIndicatorOn = true;
        }   
    }

    public void TurnLeft(){
        Debug.Log("TurnLeft called. Current state: " + leftIndicatorOn);
        if (leftIndicatorOn){
            leftIndicator.SetActive(false);
            foreach (Transform child in leftIndicator.transform) {
                child.gameObject.SetActive(false);
            }
            leftIndicatorOn = false;
        }else{
        leftIndicator.SetActive(true);
        foreach (Transform child in leftIndicator.transform) {
            child.gameObject.SetActive(true);
        }
        rightIndicator.SetActive(false);
        leftIndicatorOn = true;
        }
    }

    public void Hazard(){
        if(hazardOn){
            TurnOffAll();
            hazardOn = false;
        }else{
        // TurnOffAll();
        leftIndicator.SetActive(true);
        rightIndicator.SetActive(true);
        // leftIndicator.GetComponent<LRLights>().ResetTimer();
        // rightIndicator.GetComponent<LRLights>().ResetTimer();
        foreach (Transform child in leftIndicator.transform) {
            child.gameObject.GetComponent<LRLights>().ResetTimer();
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in rightIndicator.transform) {
            child.gameObject.GetComponent<LRLights>().ResetTimer();
            child.gameObject.SetActive(true);
        }
        hazardOn = true;
        }
    }

    //Turn off all indicators
    public void TurnOffAll (){
        leftIndicator.SetActive(false);
        rightIndicator.SetActive(false);
        leftIndicatorOn = false;
        rightIndicatorOn = false;
        hazardOn = false;
    }
}
