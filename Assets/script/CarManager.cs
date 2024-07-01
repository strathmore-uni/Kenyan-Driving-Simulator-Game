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
    
    //Slip angle for braking  
    public float slipAngle;

    //Wheel Particles
    // public WheelParticles wheelParticles;

    //Smoke prefab
    // public GameObject SmokePrefab;
    
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


    void Start()
    {
        RB.centerOfMass = CenterOfMass.transform.localPosition;
        // InstantiateSmoke();
    }


/*
    //Smoke instance method
    void InstantiateSmoke(){
        wheelParticles.FLWheelParticles = Instantiate(SmokePrefab, FLWheelCollider.transform.position-Vector3.up*FLWheelCollider.radius, Quaternion.identity, FLWheelCollider.transform).GetComponent<ParticleSystem>();
        wheelParticles.FRWheelParticles = Instantiate(SmokePrefab, FRWheelCollider.transform.position-Vector3.up*FRWheelCollider.radius, Quaternion.identity, FRWheelCollider.transform).GetComponent<ParticleSystem>();
        wheelParticles.RLWheelParticles = Instantiate(SmokePrefab, RLWheelCollider.transform.position-Vector3.up*RLWheelCollider.radius, Quaternion.identity, RLWheelMesh.transform).GetComponent<ParticleSystem>();
        wheelParticles.RRWheelParticles = Instantiate(SmokePrefab, RRWheelCollider.transform.position-Vector3.up*RRWheelCollider.radius, Quaternion.identity, RRWheelCollider.transform).GetComponent<ParticleSystem>();
    }

*/
    // Update is called once per frame
    void Update()
    {
        rpmNeedle.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(minNeedleRotation, maxNeedleRotation, RPM / (redLine * 1.1f)));
        rpmText.text = RPM.ToString("0,000") + "rpm";
        gearText.text = (gearState == GearState.Neutral) ? "N" : (currentGear + 1).ToString();
        speed = RRWheelCollider.rpm * RRWheelCollider.radius * 2f * Mathf.PI / 10f;
        speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);
        CheckInputs();
        ApplyMotor();
        ApplySteering();
        UpdateWheel();
        ApplyBrakes();
        // CheckParticles();
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
        
        // Add a damping factor to the steering angle
        float dampingFactor = 0.1f; // Adjust this value to change the damping effect
        steeringAngle *= (1 - dampingFactor);

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


/* 

void ApplySteering()
    {
        float steeringAngle = SteeringCurve.Evaluate(speed) * SteeringInput;

        // Calculate the signed angle between the car's forward direction and its velocity
        float velocityAngle = Vector3.SignedAngle(transform.forward, RB.velocity-transform.forward, Vector3.up);

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


*/
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

/*
    //Smoke when slipping
    void CheckParticles(){
        WheelHit [] wheelHits = new WheelHit[4];
        FLWheelCollider.GetGroundHit(out wheelHits[0]);
        FRWheelCollider.GetGroundHit(out wheelHits[1]);
        RLWheelCollider.GetGroundHit(out wheelHits[2]);
        RRWheelCollider.GetGroundHit(out wheelHits[3]);
        
        float slipAllowed = 0.1f;

        if (Mathf.Abs(wheelHits[0].sidewaysSlip)+Mathf.Abs(wheelHits[0].forwardSlip)>slipAllowed){
            wheelParticles.FLWheelParticles.Play();
        }else{
            wheelParticles.FLWheelParticles.Stop();
        }
        if (Mathf.Abs(wheelHits[1].sidewaysSlip)+Mathf.Abs(wheelHits[1].forwardSlip)>slipAllowed){
            wheelParticles.FRWheelParticles.Play();
        }else{
            wheelParticles.FRWheelParticles.Stop();
        }
        if (Mathf.Abs(wheelHits[2].sidewaysSlip)+Mathf.Abs(wheelHits[2].forwardSlip)>slipAllowed){
            wheelParticles.RLWheelParticles.Play();
        }else{
            wheelParticles.RLWheelParticles.Stop();
        }
        if (Mathf.Abs(wheelHits[3].sidewaysSlip)+Mathf.Abs(wheelHits[3].forwardSlip)>slipAllowed){
            wheelParticles.RRWheelParticles.Play();
        }else{
            wheelParticles.RRWheelParticles.Stop();
        }
    }

    //Wheel Particles Class
    [System .Serializable]
    public class WheelParticles{
        public ParticleSystem FLWheelParticles, FRWheelParticles, RLWheelParticles, RRWheelParticles;
    }

    */
}
