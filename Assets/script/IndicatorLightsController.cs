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
        rightIndicator.SetActive(true); // Set the parent active
        foreach (Transform child in rightIndicator.transform) {
            child.gameObject.SetActive(true); // Set each immediate child active
        }
        leftIndicator.SetActive(false);
        rightIndicatorOn = true;
    }

    public void TurnLeft(){
        leftIndicator.SetActive(true);
        foreach (Transform child in leftIndicator.transform) {
            child.gameObject.SetActive(true);
        }
        rightIndicator.SetActive(false);
        leftIndicatorOn = true;
    }

    public void Hazard(){
        leftIndicator.SetActive(true);
        rightIndicator.SetActive(true);
        foreach (Transform child in leftIndicator.transform) {
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in rightIndicator.transform) {
            child.gameObject.SetActive(true);
        }
        hazardOn = true;
    }
}
