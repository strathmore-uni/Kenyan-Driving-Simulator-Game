using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SteerAndMove : MonoBehaviour
{
    public GameObject CarBody; // CarBody GameObject
    public GameObject SteeringWheel; // SteeringWheel GameObject
    public float MoveSpeed = 5;
    public float RotationSpeed = 2;
    public float ReverseSpeed = 2;
    public float TurnSpeedValue = 2;
    public Button interiorButton; // Interior button

    private enum CarState { forward, reverse, turning }
    private CarState currentState = CarState.forward;
    private float turnSpeed = 0;
    private float carSpeed = 0;
    private bool isInteriorMode = false; // Flag to check if interior mode is active

    void Start()
    {
        // Add a listener to the interior button
        interiorButton.onClick.AddListener(ActivateInteriorMode);
    }

    void ActivateInteriorMode()
    {
        isInteriorMode = true;
    }

    void DeactivateInteriorMode()
    {
        isInteriorMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteriorMode && Input.GetMouseButton(0))
        {
            float dragDeltaX = Input.GetAxis("Mouse X");
            float dragDeltaY = Input.GetAxis("Mouse Y");

            Debug.Log("Mouse X: " + dragDeltaX);
            Debug.Log("Mouse Y: " + dragDeltaY);

            if (dragDeltaY < 0)
            {
                currentState = CarState.reverse;
            }
            else if (dragDeltaY > 0)
            {
                currentState = CarState.forward;
            }

            if (dragDeltaX < 0)
            {
                currentState = CarState.turning;
                turnSpeed = -TurnSpeedValue;
                SteeringWheel.transform.Rotate(Vector3.up, -RotationSpeed * Time.deltaTime); // Rotate steering wheel to the left
                Debug.Log("Rotating SteeringWheel to the left");
            }
            else if (dragDeltaX > 0)
            {
                currentState = CarState.turning;
                turnSpeed = TurnSpeedValue;
                SteeringWheel.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime); // Rotate steering wheel to the right
                Debug.Log("Rotating SteeringWheel to the right");
            }

            switch (currentState)
            {
                case CarState.forward:
                    carSpeed = MoveSpeed;
                    turnSpeed = 0;
                    break;
                case CarState.reverse:
                    carSpeed = -ReverseSpeed;
                    turnSpeed = 0;
                    break;
                case CarState.turning:
                    carSpeed = 0;
                    break;
            }

            Debug.Log("SteeringWheel rotation: " + SteeringWheel.transform.eulerAngles);

            CarBody.transform.Translate(translation: transform.forward * carSpeed * Time.deltaTime, relativeTo: Space.World);
            CarBody.transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
        }
    }

    void OnPointerExit(PointerEventData eventData)
    {
        DeactivateInteriorMode();
    }
}