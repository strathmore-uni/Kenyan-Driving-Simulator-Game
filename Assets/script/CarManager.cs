using System.Xml;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    // Wheel Mesh variables
    public MeshRenderer FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

    // Wheel Colliders variable
    public WheelCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

    public GameObject CenterOfMass;

    // Rigid body variable
    public Rigidbody RB;

    // Car control Inputs (Fuel, Steering)
    public float FuelInput, SteeringInput, BrakeInput;

    // Motor Inputs
    public float MotorPower, SteeringPower, BrakePower;

    // Speed
    private float speed;
    public float maxSpeed;
    private float speedClamped;

    // Steering curve
    public AnimationCurve SteeringCurve;
    public int isEngineRunning;

    // Start is called before the first frame update
    void Start()
    {
        RB.centerOfMass = CenterOfMass.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        speed = RRWheelCollider.rpm * RRWheelCollider.radius * 2f * Mathf.PI / 10f;
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);
        CheckInputs();
        ApplyMotor();
        ApplySteering();
        UpdateWheel();
        ApplyBrakes();
    }

    void CheckInputs()
    {
        if (Mathf.Abs(FuelInput) > 0 && isEngineRunning == 0)
        {
            StartCoroutine(GetComponent<EngineAudio>().StartEngine());
        }

        SteeringInput = SimpleInput.GetAxis("Horizontal");

        float MoveDir = Vector3.Dot(transform.forward, RB.velocity);
    }

    // Motor Method
    void ApplyMotor()
    {
        if (Mathf.Abs(speed) < maxSpeed)
        {
            RLWheelCollider.motorTorque = FuelInput * MotorPower;
            RRWheelCollider.motorTorque = FuelInput * MotorPower;
        }
        else
        {
            RLWheelCollider.motorTorque = 0;
            RRWheelCollider.motorTorque = 0;
        }
    }

    // Steering Method
    //Steering Method
    void ApplySteering()
    {
        float steeringAngle = SteeringCurve.Evaluate(speed) * SteeringInput;

        // Calculate the signed angle between the car's forward direction and its velocity
        float velocityAngle = Vector3.SignedAngle(transform.forward, RB.velocity, Vector3.up);

        // Adjust steering angle based on the vehicle's movement direction
        if (Vector3.Dot(transform.forward, RB.velocity) < 0)
        {
            // If reversing, invert the steering input
            steeringAngle = -steeringAngle;
        }

        // Smooth steering angle
        float currentSteerAngleFL = FLWheelCollider.steerAngle;
        float currentSteerAngleFR = FRWheelCollider.steerAngle;
        steeringAngle = Mathf.Lerp(currentSteerAngleFL, steeringAngle, Time.deltaTime * 5f);

        // Apply the smoothed and adjusted steering angle
        FLWheelCollider.steerAngle = steeringAngle;
        FRWheelCollider.steerAngle = steeringAngle;

        // Debug logs for monitoring
        Debug.Log($"Steering Input: {SteeringInput}, Speed: {speed}, Steering Angle: {steeringAngle}, Velocity Angle: {velocityAngle}");
    }


    // Wheel Update Method
    void UpdateWheel()
    {
        UpdatePos(FLWheelCollider, FLWheelMesh);
        UpdatePos(FRWheelCollider, FRWheelMesh);
        UpdatePos(RLWheelCollider, RLWheelMesh);
        UpdatePos(RRWheelCollider, RRWheelMesh);
    }

    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(Mathf.Abs(FuelInput), 0.5f, 1f);
        return speedClamped * gas / maxSpeed;
    }

    public void TakeInput(float input)
    {
        FuelInput = input;
    }

    public void TakeSteeringInput(float input)
    {
        BrakeInput = input;
    }

    // Wheel Position Update Method
    void UpdatePos(WheelCollider Col, MeshRenderer Mesh)
    {
        Vector3 Pos;
        Quaternion quar = Col.transform.rotation;

        Col.GetWorldPose(out Pos, out quar);

        Mesh.transform.position = Pos;
        Mesh.transform.rotation = quar;
    }

    void ApplyBrakes()
    {
        FLWheelCollider.brakeTorque = BrakeInput * BrakePower * .7f;
        FRWheelCollider.brakeTorque = BrakeInput * BrakePower * .7f;
        RLWheelCollider.brakeTorque = BrakeInput * BrakePower * .3f;
        RRWheelCollider.brakeTorque = BrakeInput * BrakePower * .3f;
    }
}
