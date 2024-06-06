using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{

    //Wheel Mesh variables
    public MeshRenderer FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

    //Wheel Colliders variable
    public WheelCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

    //Rigid body variable
    public Rigidbody RB;

    //Car control Inputs (Fuel, Steering)
    public float FuelInput, SteeringInput, BrakeInput;

    //Motor Inputs
    public float MotorPower, SteeringPower, BrakePower;

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
        UpdateWheel();
        ApplyBrakes();
    }

    void CheckInputs (){

        // FuelInput = Input.GetAxis("Vertical");
        SteeringInput = SimpleInput.GetAxis("Horizontal");

        float MoveDir = Vector3.Dot(transform.forward, RB.velocity);


        // if (Input.GetKey(KeyCode.Space)){
        //     BrakeInput = 1f;
        // }
        // else{
        //     BrakeInput = 0f;
        // }
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

    //Wheel Update Method
    void UpdateWheel(){
        UpdatePos(FLWheelCollider, FLWheelMesh);
        UpdatePos(FRWheelCollider, FRWheelMesh);
        UpdatePos(RLWheelCollider, RLWheelMesh);
        UpdatePos(RRWheelCollider, RRWheelMesh);
    }

    public void TakeInput(float input){
        FuelInput = input;
    }

    public void TakeSteeringInput(float input){
        BrakeInput = input;
    }

    //Wheel Position Update Method
    void UpdatePos(WheelCollider Col, MeshRenderer Mesh){
        Vector3 Pos;
        // Pos = Col.transform.position;
        Quaternion quar = Col.transform.rotation;

        Col.GetWorldPose(out Pos, out quar);

        Mesh.transform.position = Pos;
        Mesh.transform.rotation = quar;
    }

    void ApplyBrakes(){
        FLWheelCollider.brakeTorque = BrakeInput * BrakePower*.7f;
        FRWheelCollider.brakeTorque = BrakeInput * BrakePower*.7f;
        RLWheelCollider.brakeTorque = BrakeInput * BrakePower*.3f;
        RRWheelCollider.brakeTorque = BrakeInput * BrakePower*.3f;
    }

}
