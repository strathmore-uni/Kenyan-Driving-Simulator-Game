using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleInputNamespace;
using UnityEngine.EventSystems;
public enum GearState
{
    Neutral,
    Running,
    CheckingChange,
    Changing
};

public class CarManager : MonoBehaviour
{
    // Wheel Mesh variables
    public MeshRenderer FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

    // Wheel Colliders variable
    public WheelCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

    //Center of Mass
    public GameObject CenterOfMass;

    // Rigid body variable
    public Rigidbody RB;

    // Car control Inputs (Fuel, Steering, Brake)
    public float FuelInput, SteeringInput, BrakeInput;

    //gas and brake pedal buttons
    public ThrottleButton gasPedal, brakePedal;

    //Motor Power Inputs
    public float MotorPower, SteeringPower, BrakePower;

    //Speed of vehicle
    private float speed;
    public float maxSpeed;
    private float speedClamped;

    // Steering curve
    public AnimationCurve SteeringCurve;

    //Steering sensitivity
    public float steeringSensitivity = 0.5f;
    
    //Slip angle for braking  
    public float slipAngle;
    
    public int isEngineRunning;

    public float RPM;
    public float redLine;
    public float idleRPM;
    public TMP_Text rpmText;
    public TMP_Text gearText;
    public Transform rpmNeedle;
    public float minNeedleRotation;
    public float maxNeedleRotation;
    public int currentGear;

    public float[] gearRatios;
    public float differentialRatio;
    private float currentTorque;
    private float clutch;
    private float wheelRPM;
    public AnimationCurve hpToRPMCurve;
    private GearState gearState;
    public float increaseGearRPM;
    public float decreaseGearRPM;
    public float changeGearTime = 0.5f;

    public float speedKMH;


    void Start()
    {
        RB.centerOfMass = CenterOfMass.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        rpmNeedle.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(minNeedleRotation, maxNeedleRotation, RPM / (redLine * 1.1f)));
        
        // Update UI elements
        rpmText.text = speedKMH.ToString("0.0") + " km/h";
        gearText.text = (gearState == GearState.Neutral) ? "N" : (currentGear + 1).ToString();
        speedKMH = RB.velocity.magnitude *3.6f;
        speedClamped = Mathf.Lerp(speedClamped, speedKMH, Time.deltaTime);
        CheckInputs();
        ApplyMotor();
        ApplySteering();
        UpdateWheel();
        ApplyBrakes();
    }

    void CheckInputs()
    {
        FuelInput = SimpleInput.GetAxis("Vertical");
        if (gasPedal.isPressed)
        {
            FuelInput += gasPedal.dampenPress;
        }
        if (brakePedal.isPressed)
        {
            FuelInput -= brakePedal.dampenPress;
        }
        if (Mathf.Abs(FuelInput) > 0 && isEngineRunning == 0)
        {
            StartCoroutine(GetComponent<EngineAudio>().StartEngine());
        }

        SteeringInput = SimpleInput.GetAxis("Horizontal");
        //SteeringInput = SimpleInput.GetAxis("SteeringWheel");

        // Apply the steering sensitivity factor
        SteeringInput *= steeringSensitivity;

        float MoveDir = Vector3.Dot(transform.forward, RB.velocity);

        if (gearState != GearState.Changing)
        {
            if (gearState == GearState.Neutral)
            {
                clutch = 0;
                if (Mathf.Abs(FuelInput) > 0) gearState = GearState.Running;
            }
            else
            {
                clutch = Input.acceleration.y > 0.5f ? 0 : Mathf.Lerp(clutch, 1, Time.deltaTime);
            }
        }
        else
        {
            clutch = 0;
        }
        if (MoveDir < -0.5f && FuelInput > 0)
        {
            BrakeInput = Mathf.Abs(FuelInput);
        }
        else if (MoveDir > 0.5f && FuelInput < 0)
        {
            BrakeInput = Mathf.Abs(FuelInput);
        }
        else
        {
            BrakeInput = 0;
        }

    }

    // Motor Method
    void ApplyMotor()
    {
        currentTorque = CalculateTorque();
        RLWheelCollider.motorTorque = FuelInput * MotorPower;
        RRWheelCollider.motorTorque = FuelInput * MotorPower;  
    }

    float CalculateTorque()
    {
        float torque = 0;
        if (RPM < idleRPM + 200 && FuelInput == 0 && currentGear == 0)
        {
            gearState = GearState.Neutral;
        }
        if (gearState == GearState.Running && clutch > 0)
        {
            if (RPM > increaseGearRPM)
            {
                StartCoroutine(ChangeGear(1));
            }
            else if (RPM < decreaseGearRPM)
            {
                StartCoroutine(ChangeGear(-1));
            }
        }
        if (isEngineRunning > 0)
        {
            if (clutch < 0.1f)
            {
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, redLine * FuelInput) + Random.Range(-50, 50), Time.deltaTime);
            }
            else
            {
                wheelRPM = Mathf.Abs((RRWheelCollider.rpm + RLWheelCollider.rpm) / 2f) * gearRatios[currentGear] * differentialRatio;
                RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM - 100, wheelRPM), Time.deltaTime * 3f);
                torque = (hpToRPMCurve.Evaluate(RPM / redLine) * MotorPower / RPM) * gearRatios[currentGear] * differentialRatio * 5252f * clutch;
            }
        }
        return torque;
    }

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
        return RPM * gas / redLine;
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

    IEnumerator ChangeGear(int gearChange)
    {
        gearState = GearState.CheckingChange;
        if (currentGear + gearChange >= 0)
        {
            if (gearChange > 0)
            {
                //increase the gear
                yield return new WaitForSeconds(0.7f);
                if (RPM < increaseGearRPM || currentGear >= gearRatios.Length - 1)
                {
                    gearState = GearState.Running;
                    yield break;
                }
            }
            if (gearChange < 0)
            {
                //decrease the gear
                yield return new WaitForSeconds(0.1f);

                if (RPM > decreaseGearRPM || currentGear <= 0)
                {
                    gearState = GearState.Running;
                    yield break;
                }
            }
            gearState = GearState.Changing;
            yield return new WaitForSeconds(changeGearTime);
            currentGear += gearChange;
        }

        if (gearState != GearState.Neutral)
            gearState = GearState.Running;
    }

    void ApplyBrakes()
    {
        FLWheelCollider.brakeTorque = BrakeInput * BrakePower * .7f;
        FRWheelCollider.brakeTorque = BrakeInput * BrakePower * .7f;
        RLWheelCollider.brakeTorque = BrakeInput * BrakePower * .3f;
        RRWheelCollider.brakeTorque = BrakeInput * BrakePower * .3f;
    }

}
