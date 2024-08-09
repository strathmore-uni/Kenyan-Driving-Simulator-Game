using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 10f;
    private bool isRotating = false;
    private float startMousePosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            startMousePosition = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }
        if (isRotating)
        {
            float currentMousePosition = Input.mousePosition.x;
            float mouseMovement = currentMousePosition - startMousePosition;

            float rotation = mouseMovement * Speed * Time.deltaTime;
            if (rotation > 0)
            {
                transform.Rotate(transform.forward, rotation);
            }
            else
            {
                transform.Rotate(transform.forward, rotation * -1);
            }
            startMousePosition = currentMousePosition;
        }
    }

    
}
