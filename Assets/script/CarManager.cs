using SimpleInputNamespace;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
public enum GearState
{
    Neutral,
    Running,
    CheckingChange,
    Changing
};

public class CarManager : MonoBehaviour
{

    //Wheel Mesh variables
    public MeshRenderer FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

    //Wheel Colliders variable
    public WheelCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

    public GameObject CenterOfMass;

    //Rigid body variable
    public Rigidbody RB;
    public float speed;
    

    //Car control Inputs (Fuel, Steering)
    public float FuelInput, SteeringInput, BrakeInput;
    public int isEngineRunning;

    //Motor Inputs
    public float MotorPower, SteeringPower, BrakePower;
    public float RPM;
    public float redLine;
    public float idleRPM;
    public TMP_Text rpmText;
    public TMP_Text gearText;
    public Transform rpmNeedle;
    public int currentGear;
    public float minNeedleRotation;
    public float maxNeedleRotation;

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

    public GameObject tireTrail;
   
    // Start is called before the first frame update
    void Start()
    {
        RB.centerOfMass = CenterOfMass.transform.localPosition;
        InitiateParticles();
        
    }

    void InitiateParticles()
    {
        //if (smokePrefab)
        //{
        //    wheelParticles.FRWheel = Instantiate(smokePrefab, FRWheelCollider.transform.position - Vector3.up * FRWheelCollider.radius, Quaternion.identity, FRWheelCollider.transform)
        //        .GetComponent<ParticleSystem>();
        //    wheelParticles.FLWheel = Instantiate(smokePrefab, FLWheelCollider.transform.position - Vector3.up * FRWheelCollider.radius, Quaternion.identity, FLWheelCollider.transform)
        //        .GetComponent<ParticleSystem>();
        //    wheelParticles.RRWheel = Instantiate(smokePrefab, RRWheelCollider.transform.position - Vector3.up * FRWheelCollider.radius, Quaternion.identity, RRWheelCollider.transform)
        //        .GetComponent<ParticleSystem>();
        //    wheelParticles.RLWheel = Instantiate(smokePrefab, RLWheelCollider.transform.position - Vector3.up * FRWheelCollider.radius, Quaternion.identity, RLWheelCollider.transform)
        //        .GetComponent<ParticleSystem>();
        //}
        if (tireTrail)
        {
            Instantiate(tireTrail, FRWheelCollider.transform.position - Vector3.up * FRWheelCollider.radius, Quaternion.identity, FRWheelCollider.transform);
            Instantiate(tireTrail, FLWheelCollider.transform.position - Vector3.up * FRWheelCollider.radius, Quaternion.identity, FLWheelCollider.transform);
            Instantiate(tireTrail, RRWheelCollider.transform.position - Vector3.up * FRWheelCollider.radius, Quaternion.identity, RRWheelCollider.transform);
            Instantiate(tireTrail, RLWheelCollider.transform.position - Vector3.up * FRWheelCollider.radius, Quaternion.identity, RLWheelCollider.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rpmNeedle.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(minNeedleRotation, maxNeedleRotation, RPM / (redLine * 1.1f)));
        rpmText.text = RPM.ToString("0,000") + "rpm";
        gearText.text = (gearState == GearState.Neutral) ? "N" : (currentGear + 1).ToString();
        speed = RRWheelCollider.rpm * RRWheelCollider.radius * 2f * Mathf.PI / 10f;

        CheckInputs();
        ApplyMotor();
        ApplySteering();  
        UpdateWheel();
        ApplyBrakes();
    }

    void CheckInputs (){
        SteeringInput = SimpleInput.GetAxis("Horizontal");

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
                clutch = Input.GetKey(KeyCode.LeftShift) ? 0 : Mathf.Lerp(clutch, 1, Time.deltaTime);
            }
        }
        else
        {
            clutch = 0;
        }
        //if (movingDirection < -0.5f && FuelInput > 0)
        //{
        //    brakeInput = Mathf.Abs(FuelInput);
        //}
        //else if (movingDirection > 0.5f && FuelInput < 0)
        //{
        //    brakeInput = Mathf.Abs(FuelInput);
        //}
        //else
        //{
        //    brakeInput = 0;
        //}
    }

    //Motor Method
    void ApplyMotor(){
        currentTorque = CalculateTorque();
        RLWheelCollider.motorTorque = FuelInput * currentTorque;
        RRWheelCollider.motorTorque = FuelInput * currentTorque;
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
    void ApplySteering(){
        FLWheelCollider.steerAngle = SteeringInput * SteeringPower;
        FRWheelCollider.steerAngle = SteeringInput * SteeringPower;
    }

    void CheckParticles()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        FRWheelCollider.GetGroundHit(out wheelHits[0]);
        FLWheelCollider.GetGroundHit(out wheelHits[1]);
        RRWheelCollider.GetGroundHit(out wheelHits[2]);
        RLWheelCollider.GetGroundHit(out wheelHits[3]);

        float slipAllowance = 0.2f;

        if ((Mathf.Abs(wheelHits[0].sidewaysSlip) + Mathf.Abs(wheelHits[0].forwardSlip) > slipAllowance))
        {
            // Play particle effect for FR wheel
        }
        else
        {
            // Stop particle effect for FR wheel
        }

        if ((Mathf.Abs(wheelHits[1].sidewaysSlip) + Mathf.Abs(wheelHits[1].forwardSlip) > slipAllowance))
        {
            // Play particle effect for FL wheel
        }
        else
        {
            // Stop particle effect for FL wheel
        }

        if ((Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip) > slipAllowance))
        {
            // Play particle effect for RR wheel
        }
        else
        {
            // Stop particle effect for RR wheel
        }

        if ((Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip) > slipAllowance))
        {
            // Play particle effect for RL wheel
        }
        else
        {
            // Stop particle effect for RL wheel
        }
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
        Quaternion quar = Col.transform.rotation;

        Col.GetWorldPose(out Pos, out quar);

        Mesh.transform.position = Pos;
        Mesh.transform.rotation = quar;
    }

    public float GetSpeedRatio()
    {
        var gas = Mathf.Clamp(Mathf.Abs(FuelInput), 0.5f, 1f);
        return RPM * gas / redLine;
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


void ApplyBrakes(){
        FLWheelCollider.brakeTorque = BrakeInput * BrakePower*.7f;
        FRWheelCollider.brakeTorque = BrakeInput * BrakePower*.7f;
        RLWheelCollider.brakeTorque = BrakeInput * BrakePower*.3f;
        RRWheelCollider.brakeTorque = BrakeInput * BrakePower*.3f;
    }

}

[System.Serializable]
public class WheelParticles
{
    public ParticleSystem FRWheel;
    public ParticleSystem FLWheel;
    public ParticleSystem RRWheel;
    public ParticleSystem RLWheel;

    public TrailRenderer FRWheelTrail;
    public TrailRenderer FLWheelTrail;
    public TrailRenderer RRWheelTrail;
    public TrailRenderer RLWheelTrail;

}
