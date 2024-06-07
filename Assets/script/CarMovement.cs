using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 20.0f;
    public float turnSpeed = 100.0f;
    public float HorizontalInput;
    public float ForwardInput;

    private Rigidbody rb;


//Gase and Brake input
    public int GasInput;
    public int BrakeInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component of the car
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalInput = SimpleInput.GetAxis("Horizontal");
        ForwardInput = Input.GetAxis("Vertical");

        // Calculate the car's current speed
        float currentSpeed = rb.velocity.magnitude;


    }


// Gas and brake methods
    public void GasPressed(){
        GasInput = 1;
    }

    public void GasReleased(){
        GasInput = 0;
    }

    public void BrakePressed(){
        BrakeInput = 1;
    }

    public void BrakeReleased(){
        BrakeInput = 0;
    }

    void FixedUpdate()
    {
        // Move the car forward with physics
        Vector3 moveDirection = transform.forward * ForwardInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        // Turn the car based on horizontal input
        float turn = HorizontalInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
