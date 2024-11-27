using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorSteering : MonoBehaviour
{
    private float SceneWidth;
    private Vector3 PressPoint;
    private Quaternion StartRotation;
    // Start is called before the first frame update
    void Start()
    {
        SceneWidth = Screen.width; 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            PressPoint = Input.mousePosition;
            StartRotation = transform.rotation;
        }
        else if (Input.GetMouseButton(0))
        {
            float CurrentDistanceBetweenMousePositions = (Input.mousePosition - PressPoint).x;
            transform.rotation = StartRotation * Quaternion.Euler(Vector3.forward * (CurrentDistanceBetweenMousePositions / SceneWidth * 360));
        }
    }
}
