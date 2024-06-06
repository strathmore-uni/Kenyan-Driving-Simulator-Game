using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{

    //Wheel Mesh variables
    public Transform FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

    //Wheel Colliders variable
    public WheelCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

    //Rigid body variable
    public Rigidbody RB;

    //Car control Inputs (Fuel, Steering)
    public float FuelInput, SteeringInput;

    //Motor Inputs
    public float MotorPower;
    public float SteeringPower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
        ApplyMotor();
        ApplySteering();  
    }

    void CheckInputs (){
        FuelInput = Input.GetAxis("Vertical");
        SteeringInput = Input.GetAxis("Horizontal");
    }

    //Motor Method
    void ApplyMotor(){
        RLWheelCollider.motorTorque = FuelInput * MotorPower;
        RRWheelCollider.motorTorque = FuelInput * MotorPower;
    }

    //Steering Method
    void ApplySteering(){
        FLWheelCollider.steerAngle = SteeringInput * SteeringPower;
        FRWheelCollider.steerAngle = SteeringInput * SteeringPower;
    }

}
