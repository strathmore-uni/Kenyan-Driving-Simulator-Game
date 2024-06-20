using UnityEngine;

public class CarManager : MonoBehaviour
{

    //Wheel Mesh variables
    public MeshRenderer FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

    //Wheel Colliders variable
    public WheelCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

    public GameObject CenterOfMass;

    //Rigid body variable
    public Rigidbody RB;

    //Car control Inputs (Fuel, Steering)
    public float FuelInput, SteeringInput, BrakeInput;

    //Motor Inputs
    public float MotorPower, SteeringPower, BrakePower;

    //Speed
    private float speed;

    //Steering curve
    public AnimationCurve SteeringCurve;

    //Wheel smoke particles
    // public ParticleSystem FLWheelSmoke, FRWheelSmoke, RLWheelSmoke, RRWheelSmoke;

    //smoke prefab
    // public GameObject SmokePrefab;

    // Start is called before the first frame update
    void Start()
    {
        RB.centerOfMass = CenterOfMass.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        speed = RB.velocity.magnitude;
        CheckInputs();
        ApplyMotor();
        ApplySteering();  
        UpdateWheel();
        ApplyBrakes();
    }

    void CheckInputs (){
        SteeringInput = SimpleInput.GetAxis("Horizontal");

        float MoveDir = Vector3.Dot(transform.forward, RB.velocity);
    }

/*
    //Wheel Smoke Method
    void WheelSmoke(ParticleSystem Smoke, WheelCollider Wheel){
        if(Wheel.rpm < 0){
            Smoke.transform.position = Wheel.transform.position - Wheel.transform.up * Wheel.radius;
            Smoke.transform.rotation = Wheel.transform.rotation;
            if(!Smoke.isPlaying){
                Smoke.Play();
            }
        }else{
            Smoke.Stop();
        }
    }
*/
    //Motor Method
    void ApplyMotor(){
        RLWheelCollider.motorTorque = FuelInput * MotorPower;
        RRWheelCollider.motorTorque = FuelInput * MotorPower;
    }

    //Steering Method
    void ApplySteering(){
        float steeringAngle = SteeringCurve.Evaluate(speed) * SteeringInput;
        FLWheelCollider.steerAngle = steeringAngle;
        FRWheelCollider.steerAngle = steeringAngle;
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

    void ApplyBrakes(){
        FLWheelCollider.brakeTorque = BrakeInput * BrakePower*.7f;
        FRWheelCollider.brakeTorque = BrakeInput * BrakePower*.7f;
        RLWheelCollider.brakeTorque = BrakeInput * BrakePower*.3f;
        RRWheelCollider.brakeTorque = BrakeInput * BrakePower*.3f;
    }

}
