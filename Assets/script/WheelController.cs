using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour

{
    //Wheel colliders
    [SerializeField] WheelCollider FrontLeftWheelCollider;
    [SerializeField] WheelCollider FrontRightWheel;
    [SerializeField] WheelCollider RearLeftWheel;
    [SerializeField] WheelCollider RearRightWheel;

//Wheels meshes
    [SerializeField] Transform FrontLeftWheelTransform;
    [SerializeField] Transform FrontRightWheelTransform;
    [SerializeField] Transform RearLeftWheelTransform;
    [SerializeField] Transform RearRightWheelTransform;

    public float acceleration = 500f;
    public float brakingForce = 400f;
    public float maxTurnAngle = 15f;


    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurnAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Get forawrd/reverse acceleration from the vertical axis (W and S keys)
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        //if we'are pressing space, give currentbrakingForce a value
        if (Input.GetKey(KeyCode.Space))
            currentBrakeForce = brakingForce;
        else
            currentBrakeForce = 0f;

        //Apply acceleration to front wheels;
        FrontLeftWheelCollider.motorTorque = currentAcceleration;
        FrontRightWheel.motorTorque = currentAcceleration;

        //Apply breaking force to all wheels
        FrontLeftWheelCollider.brakeTorque = currentBrakeForce;
        FrontRightWheel.brakeTorque = currentBrakeForce;
        RearLeftWheel.brakeTorque = currentBrakeForce;
        RearRightWheel.brakeTorque = currentBrakeForce;

        //take care of steering
        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        FrontLeftWheelCollider.steerAngle = currentTurnAngle;
        FrontRightWheel.steerAngle = currentTurnAngle;

        //update wheel meshes
        UpdateWheel(FrontLeftWheelCollider, FrontLeftWheelTransform);
        UpdateWheel(FrontRightWheel, FrontRightWheelTransform);
        UpdateWheel(RearLeftWheel, RearLeftWheelTransform);
        UpdateWheel(RearRightWheel, RearRightWheelTransform);
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        //get wheelcollidr state
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        //set wheel transform state
        trans.position = position;
        trans.rotation = rotation;

    }
}
