using UnityEngine;

public class CarManager : MonoBehaviour
{

    //Wheel Mesh variables
    public MeshRenderer FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

    //Wheel Colliders variable
    public WheelCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

    //Center of Mass
    public GameObject CenterOfMass;

    //Rigid body variable
    public Rigidbody RB;

    //Car control Inputs (Fuel, Steering, Brake)
    public float FuelInput, SteeringInput, BrakeInput;

    //gas and brake pedal buttons
    public ThrottleButton gasPedal, brakePedal;

    //Motor Power Inputs
    public float MotorPower, SteeringPower, BrakePower;

    //Speed of vehicle
    private float speed;

    //Steering curve
    public AnimationCurve SteeringCurve;

    //Slip angle for braking  
    public float slipAngle;

    //Wheel Particles
    // public WheelParticles wheelParticles;

    //Smoke prefab
    // public GameObject SmokePrefab;

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
        speed = RB.velocity.magnitude;
        CheckInputs();
        ApplyMotor();
        ApplySteering();  
        UpdateWheel();
        ApplyBrakes();
        // CheckParticles();
    }

    void CheckInputs (){
        SteeringInput = SimpleInput.GetAxis("Horizontal");
        if (gasPedal.isPressed){
            FuelInput += gasPedal.dampenPress;
            Debug.Log($"Gas Pedal Pressed and fuel input is {FuelInput}");
        }
        if (brakePedal.isPressed){
            BrakeInput -= brakePedal.dampenPress;
            Debug.Log($"Brake Pedal Pressed and brake input is {BrakeInput}");
        }

        // float MoveDir = Vector3.Dot(transform.forward, RB.velocity);
        slipAngle = Vector3.Angle(transform.forward, RB.velocity);
        if(slipAngle>120){
            if(FuelInput<0){
                BrakeInput = Mathf.Abs(FuelInput);
                FuelInput = 0;
            }
        }else{
                BrakeInput = 0;
            }
    }

    //Motor Method
    void ApplyMotor(){
        RLWheelCollider.motorTorque = FuelInput * MotorPower;
        RRWheelCollider.motorTorque = FuelInput * MotorPower;
    }

    //Steering Method
    void ApplySteering(){
        float steeringAngle = SteeringCurve.Evaluate(speed) * SteeringInput;
        steeringAngle += Vector3.SignedAngle(transform.forward, RB.velocity-transform.forward, Vector3.up);
        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90);
        FLWheelCollider.steerAngle = steeringAngle;
        FRWheelCollider.steerAngle = steeringAngle;
    }

/* 

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

*/
    //Wheel Update Method
    void UpdateWheel(){
        UpdatePos(FLWheelCollider, FLWheelMesh);
        UpdatePos(FRWheelCollider, FRWheelMesh);
        UpdatePos(RLWheelCollider, RLWheelMesh);
        UpdatePos(RRWheelCollider, RRWheelMesh);
    }

/*
    public void TakeInput(float input){
        FuelInput = input;
    }

    public void TakeSteeringInput(float input){
        BrakeInput = input;
    }

*/
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
