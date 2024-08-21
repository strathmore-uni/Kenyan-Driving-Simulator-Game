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
    Changing,
    Reverse
}

namespace MyNamespace
{
    public class CarManager : MonoBehaviour
    {
        // Wheel Mesh variables
        public MeshRenderer FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

        // Wheel Colliders variable
        public SphereCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

        // Center of Mass
        public GameObject CenterOfMass;

        // Rigid body variable
        public Rigidbody RB;

        // Car control Inputs (Fuel, Steering, Brake)
        public float FuelInput, SteeringInput, BrakeInput;

        // Gas and brake pedal buttons
        public ThrottleButton gasPedal, brakePedal, interiorGasPedal, interiorBrakePedal;

        // Motor Power Inputs
        public float MotorPower, SteeringPower, BrakePower;

        // Speed of vehicle
        private float speed;
        public float maxSpeed;
        private float speedClamped;

        // Steering curve
        public AnimationCurve SteeringCurve;

        // Steering sensitivity
        public float steeringSensitivity = 0.5f;

        // Slip angle for braking  
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

        public Transform interiorSpeedometerNeedle; // Interior speedometer needle transform
        public TMP_Text interiorSpeedText; // Interior speed display text
        public TMP_Text interiorGearText;  // Interior gear display text

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
        public float turningSpeed = 5f;
        public float turnSpeed = 5f;
        private Animator charAnim;
        private float animatorTurnAngle;

        private SphereCollider frontLeftWheel; // Front left wheel collider
        private SphereCollider frontRightWheel; // Front right wheel collider

        public float maxSteerAngle = 30f;
        public float steerSpeed = 2f;
        private float currentSteerAngle = 0f;

        public float suspensionDistance = 0.2f;
        public float suspensionSpring = 20000f;
        public float suspensionDamping = 4500f;
        void Start()
        {
            RB = GetComponent<Rigidbody>();
            RB.centerOfMass = CenterOfMass.transform.localPosition;
            Cursor.visible = true;
            RB.centerOfMass = new Vector3(0, -0.9f, 0);  // Adjust the Y-value lower to make the car more stable.
        }

        //void ApplySteering(float steerAngle)
        //{
        //    // Apply the steering to the car
        //    float adjustedSteerAngle = steerAngle * SteeringCurve.Evaluate(speedClamped);
        //    frontLeftWheel.transform.localRotation = Quaternion.Euler(0, adjustedSteerAngle, 0);
        //    frontRightWheel.transform.localRotation = Quaternion.Euler(0, adjustedSteerAngle, 0);
        //}
        // Update is called once per frame
        void Update()
        {
            float speedRatio = speedKMH / (maxSpeed * 1.1f);
            float needleRotation = Mathf.Lerp(minNeedleRotation, maxNeedleRotation, speedRatio);
            rpmNeedle.rotation = Quaternion.Euler(0, 0, needleRotation);

            // Update UI elements
            rpmText.text = speedKMH.ToString("0.0") + " km/h";
            gearText.text = (gearState == GearState.Neutral) ? "N" : (currentGear + 1).ToString();

            interiorSpeedText.text = speedKMH.ToString("0.0") + " km/h";
            interiorGearText.text = (gearState == GearState.Neutral) ? "N" : (currentGear + 1).ToString();

            speedKMH = RB.velocity.magnitude * 3.6f;
            speedClamped = Mathf.Lerp(speedClamped, speedKMH, Time.deltaTime);

            // Update interior speedometer
            float interiorNeedleRotation = Mathf.Lerp(minNeedleRotation, maxNeedleRotation, speedRatio);
            interiorSpeedometerNeedle.rotation = Quaternion.Euler(0, 0, interiorNeedleRotation);

            CheckInputs();
            ApplyMotor();
            //ApplySteering();
            UpdateWheel();
            ApplyBrakes();
            //ApplySuspension();

            // Debug logs to check input
            Debug.Log($"Steering Input: {SimpleInput.GetAxis("Horizontal")}");

            // Get the input
            float steerInput = SimpleInput.GetAxis("Horizontal"); // You might use touch input for mobile

            // Calculate the target angle
            float targetSteerAngle = steerInput * maxSteerAngle;

            // Smoothly steer the car
            currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * steerSpeed);

            // Apply the steering to the car
            /*ApplySteering(currentSteerAngle)*/;
        }
        

        //void ApplySuspension()
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(RLWheelCollider.transform.position, -transform.up, out hit, suspensionDistance))
        //    {
        //        Vector3 velocity = RB.GetPointVelocity(hit.point);
        //        float compression = 1.0f - (hit.distance / suspensionDistance);

        //        Vector3 suspensionForce = transform.up * (compression * suspensionSpring) - velocity * suspensionDamping;

        //        RB.AddForceAtPosition(suspensionForce, RLWheelCollider.transform.position);
        //        RB.AddForceAtPosition(suspensionForce, RRWheelCollider.transform.position);
        //    }
        //}

        void Move(float direction)
        {
            // Example of moving the car
            transform.Translate(Vector3.forward * speed * direction * Time.deltaTime);
        }

        void ApplySteering(float steerAngle)
        {
            // Apply the steering to the car
            float adjustedSteerAngle = steerAngle * SteeringCurve.Evaluate(speed);
            frontLeftWheel.transform.localRotation = Quaternion.Euler(0, adjustedSteerAngle, 0);
            frontRightWheel.transform.localRotation = Quaternion.Euler(0, adjustedSteerAngle, 0);
        }

        void ApplyMovement(float speed)
        {
            // Apply the movement to the car
           RB.velocity = transform.forward * speed;
        }

        //void FixedUpdate()
        //{
        //    ActivateLights();
        //    animatorTurnAngle = Mathf.Lerp(animatorTurnAngle, -SimpleInput.GetAxis("Horizontal"), 28f * Time.deltaTime);
        //    charAnim.SetFloat("turnAngle", animatorTurnAngle);
        //}

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

            // Interior inputs (newly added)
            if (interiorGasPedal.isPressed)
            {
                FuelInput -= interiorGasPedal.dampenPress; // Change sign from + to -
            }
            if (interiorBrakePedal.isPressed)
            {
                FuelInput += interiorBrakePedal.dampenPress; // Change sign from - to +
            }

            if (Mathf.Abs(FuelInput) > 0 && isEngineRunning == 0)
            {
                StartCoroutine(GetComponent<EngineAudio>().StartEngine());
            }

            SteeringInput = SimpleInput.GetAxis("Horizontal");

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
        public float GetSpeedRatio()
        {
            var gas = Mathf.Clamp(Mathf.Abs(FuelInput), 0.5f, 1f);
            return RPM * gas / redLine;
        }

        void ApplyMotor()
        {
            currentTorque = CalculateTorque();
            // Update: Use Rigidbody.AddTorque instead of WheelCollider.motorTorque
            RB.AddTorque(transform.up * FuelInput * MotorPower, ForceMode.VelocityChange);
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
                if (clutch < 0.5f)
                {
                    torque = MotorPower;
                }
            }
            return torque;
        }

        void ApplyBrakes()
        {
            BrakeInput = Mathf.Clamp01(BrakeInput);
            // Update: Use Rigidbody.AddForce instead of WheelCollider.brakeTorque
            RB.AddForce(-transform.forward * BrakeInput * BrakePower, ForceMode.VelocityChange);
        }
        void UpdateWheel()
        {
            // Correctly call UpdateWheelMesh with the corresponding SphereCollider and MeshRenderer
            UpdateWheelMesh(FLWheelCollider, FLWheelMesh);
            UpdateWheelMesh(FRWheelCollider, FRWheelMesh);
            UpdateWheelMesh(RLWheelCollider, RLWheelMesh);
            UpdateWheelMesh(RRWheelCollider, RRWheelMesh);
        }


        void UpdateWheelMesh(SphereCollider wheelCollider, MeshRenderer wheelMesh)
        {
            wheelMesh.transform.position = wheelCollider.transform.position;
            wheelMesh.transform.rotation = wheelCollider.transform.rotation;
        }


        void ActivateLights()
        {
            if (Input.GetKey(KeyCode.L))
            {
                // Logic to activate lights
            }
        }

        
    


        IEnumerator ChangeGear(int gearChange)
        {
            gearState = GearState.CheckingChange;
            if (currentGear + gearChange >= 0)
            {
                if (gearChange > 0)
                {
                    // Increase the gear
                    yield return new WaitForSeconds(0.7f);
                    if (RPM < increaseGearRPM || currentGear >= gearRatios.Length - 1)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                if (gearChange < 0)
                {
                    // Decrease the gear
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

        //void ApplyBrakes()
        //{
        //    FLWheelCollider.brakeTorque = BrakeInput * BrakePower * .7f;
        //    FRWheelCollider.brakeTorque = BrakeInput * BrakePower * .7f;
        //    RLWheelCollider.brakeTorque = BrakeInput * BrakePower * .3f;
        //    RRWheelCollider.brakeTorque = BrakeInput * BrakePower * .3f;
        //}

        
        //void ActivateLights()
        //{
        //    // Add your lights activation logic here
        //}
    }
}
