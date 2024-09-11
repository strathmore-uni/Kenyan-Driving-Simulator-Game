using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleInputNamespace;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum GearState
{
    Park,
    Neutral,
    Running,
    Drive,
    Reverse,
    CheckingChange,
    Changing
}

namespace MyNamespace
{
    public class CarManager : MonoBehaviour
    {
        // Wheel Mesh variables
        public MeshRenderer FLWheelMesh, FRWheelMesh, RLWheelMesh, RRWheelMesh;

        // Wheel Colliders variable
        public WheelCollider FLWheelCollider, FRWheelCollider, RLWheelCollider, RRWheelCollider;

        // Center of Mass
        public GameObject CenterOfMass;

        // Rigid body variable
        public Rigidbody RB;

        // Car control Inputs (Fuel, Steering, Brake)
        public float FuelInput, SteeringInput, BrakeInput;

        // Gas and brake pedal buttons
        public ThrottleButton gasPedal, brakePedal;

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

        public WheelCollider frontLeftWheel; // Front left wheel collider
        public WheelCollider frontRightWheel; // Front right wheel collider

        public float maxSteerAngle = 30f;
        public float steerSpeed = 2f;
        private float currentSteerAngle = 0f;

        public float accelerationForce = 10.0f; // adjust this value to your liking
        public float reverseSpeed = 5.0f; // adjust this value to your liking
        public float forwardSpeed = 20.0f; // adjust this value to your liking
        public float brakingForce = 10.0f; // adjust this value to your liking

        public Button parkButton;
        public Button neutralButton;
        public Button driveButton;
        public Button reverseButton;

        void Start()
        {
            RB.centerOfMass = CenterOfMass.transform.localPosition;
            RB = GetComponent<Rigidbody>();
            Cursor.visible = true;
            RB.centerOfMass = new Vector3(0, -0.9f, 0);  // Adjust the Y-value lower to make the car more stable.

            parkButton.onClick.AddListener(ParkGear);
            neutralButton.onClick.AddListener(NeutralGear);
            driveButton.onClick.AddListener(DriveGear);
            reverseButton.onClick.AddListener(ReverseGear);
        }
        public void ParkGear()
        {
            gearState = GearState.Park;
            currentGear = 0;
            // Stop the car
            RB.velocity = Vector3.zero;
        }

        public void NeutralGear()
        {
            gearState = GearState.Neutral;
            currentGear = 0;
            // Apply brakes to stop the car gradually
            ApplyBrakes();
        }
        public void DriveGear()
        {
            gearState = GearState.Drive;
            currentGear = 1;
        }

        public void ReverseGear()
        {
            gearState = GearState.Reverse;
            currentGear = -1;
        }

        public void Steer(float steerAngle)
        {
            // Clamp the steerAngle to the range of -maxSteerAngle to maxSteerAngle
            steerAngle = Mathf.Clamp(steerAngle, -maxSteerAngle, maxSteerAngle);

            // Apply the steering angle to the front wheels
            frontLeftWheel.steerAngle = steerAngle;
            frontRightWheel.steerAngle = steerAngle;
        }

        // Update is called once per frame
        void Update()
        {
            float speedRatio = speedKMH / (maxSpeed * 1.1f);
            float needleRotation = Mathf.Lerp(minNeedleRotation, maxNeedleRotation, speedRatio);
            rpmNeedle.rotation = Quaternion.Euler(0, 0, needleRotation);

            // Update UI elements
            rpmText.text = speedKMH.ToString("0.0") + " km/h";
            gearText.text = (gearState == GearState.Neutral) ? "N" : (currentGear + 1).ToString();



            speedKMH = RB.velocity.magnitude * 3.6f;
            speedClamped = Mathf.Lerp(speedClamped, speedKMH, Time.deltaTime);

            // Update interior speedometer


            CheckInputs();
            ApplyMotor();
            //ApplySteering();
            UpdateWheel();
            ApplyBrakes();

            // Debug logs to check input
            Debug.Log($"Steering Input: {SimpleInput.GetAxis("Horizontal")}");

            // Get the input
            float steerInput = SimpleInput.GetAxis("Horizontal"); // You might use touch input for mobile

            // Calculate the target angle
            float targetSteerAngle = steerInput * maxSteerAngle;

            // Smoothly steer the car
            currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * steerSpeed);

            // Apply the steering to the car
            ApplySteering(currentSteerAngle);
        }

        void Move(float direction)
        {
            // Example of moving the car
            transform.Translate(Vector3.forward * speed * direction * Time.deltaTime);
        }

        void ApplySteering(float steerAngle)
        {
            // Apply the steering to the car
            float adjustedSteerAngle = steerAngle * SteeringCurve.Evaluate(speedClamped);
            frontLeftWheel.steerAngle = adjustedSteerAngle;
            frontRightWheel.steerAngle = adjustedSteerAngle;

            // Ensure the steering is smooth and does not exceed max angle
            frontLeftWheel.steerAngle = Mathf.Clamp(frontLeftWheel.steerAngle, -maxSteerAngle, maxSteerAngle);
            frontRightWheel.steerAngle = Mathf.Clamp(frontRightWheel.steerAngle, -maxSteerAngle, maxSteerAngle);
        }
        void CheckInputs(out float fuelInput, out float brakeInput)
        {
            // Your input checking logic here
            fuelInput = SimpleInput.GetAxis("Vertical");
            brakeInput = SimpleInput.GetAxis("Brake");
        }

        void FixedUpdate()
        {
            ActivateLights();
            animatorTurnAngle = Mathf.Lerp(animatorTurnAngle, -SimpleInput.GetAxis("Horizontal"), 28f * Time.deltaTime);
            charAnim.SetFloat("turnAngle", animatorTurnAngle);

            float fuelInput;
            float brakeInput;
            CheckInputs(out fuelInput, out brakeInput);

            if (fuelInput > 0.0f)
            {
                Accelerate(fuelInput);
            }
            else if (brakeInput > 0.0f)
            {
                Brake(brakeInput);
            }
            else if (fuelInput < 0.0f)
            {
                Reverse(-fuelInput);
            }
            else
            {
                SlowDown();
            }

            if (gearState == GearState.Park || gearState == GearState.Neutral)
            {
                // When in Park or Neutral, apply brakes and do not apply motor torque
                ApplyBrakes();
                return; // Exit FixedUpdate early
            }
        }



        public void Accelerate(float input)
        {
            // Apply a force in the direction of the car's forward direction
            RB.AddForce(transform.forward * accelerationForce * input, ForceMode.Acceleration);

            // Limit the forward speed
            if (RB.velocity.magnitude > forwardSpeed)
            {
                RB.velocity = Vector3.ClampMagnitude(RB.velocity, forwardSpeed);
            }
        }

        public void Brake(float input)
        {
            // Apply a force in the opposite direction of the car's velocity
            RB.AddForce(-RB.velocity.normalized * brakingForce * input, ForceMode.Acceleration);

            // Limit the forward speed
            if (RB.velocity.magnitude > forwardSpeed)
            {
                RB.velocity = Vector3.ClampMagnitude(RB.velocity, forwardSpeed);
            }
        }

        public void Reverse(float input)
        {
            // Apply a force in the opposite direction of the car's forward direction
            RB.AddForce(-transform.forward * accelerationForce * input, ForceMode.Acceleration);

            // Limit the reverse speed
            if (RB.velocity.magnitude > reverseSpeed)
            {
                RB.velocity = Vector3.ClampMagnitude(RB.velocity, reverseSpeed);
            }
        }

        public void SlowDown()
        {
            // Apply a force in the opposite direction of the car's velocity
            RB.AddForce(-RB.velocity.normalized * brakingForce, ForceMode.Acceleration);
        }
        public void Idle()
        {
            // Implement idling logic here
            Debug.Log("Idling");
        }


        float doubleClickTime = 0.5f; // Time in seconds to consider a double click
        float lastBrakePressTime = 0f;

        void CheckInputs()
        {
            FuelInput = SimpleInput.GetAxis("Vertical");
            if (gasPedal.isPressed)
            {
                FuelInput += gasPedal.dampenPress;
            }

            if (brakePedal.isPressed)
            {
                // Override fuel input with 0 when brake pedal is pressed
                FuelInput = 0;
                // Use brake pedal dampenPress to adjust braking force
                BrakeInput = brakePedal.dampenPress;
                //// Check for double click
                //float currentTime = Time.time;
                //if (currentTime - lastBrakePressTime < doubleClickTime)
                //{
                //    // Double click detected, apply emergency brake
                //    FuelInput = -1f; // Set FuelInput to maximum braking value
                //}
                //lastBrakePressTime = currentTime;
            }


            SteeringInput = SimpleInput.GetAxis("Horizontal");

            // Apply the steering sensitivity factor
            SteeringInput *= steeringSensitivity;

            float MoveDir = Vector3.Dot(transform.forward, RB.velocity);

            // Add gear shifting logic
            //if (parkButton.isPressed) // Park gear
            //{
            //    gearState = GearState.Park;
            //    currentGear = 0;
            //}
            //else if (neutralButton.isPressed) // Neutral gear
            //{
            //    gearState = GearState.Neutral;
            //    currentGear = 0;
            //}
            //else if (driveButton.isPressed) // Drive gear
            //{
            //    gearState = GearState.Drive;
            //    currentGear = 1;
            //}
            //else if (reverseButton.isPressed) // Reverse gear
            //{
            //    gearState = GearState.Reverse;
            //    currentGear = -1;
            //}

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

        public void ApplyMotor()
        {
            currentTorque = CalculateTorque();
            if (gearState == GearState.Drive)
            {
                RLWheelCollider.motorTorque = FuelInput * MotorPower;
                RRWheelCollider.motorTorque = FuelInput * MotorPower;
            }
            else if (gearState == GearState.Reverse)
            {
                RLWheelCollider.motorTorque = -FuelInput * MotorPower;
                RRWheelCollider.motorTorque = -FuelInput * MotorPower;
            }
            else
            {
                RLWheelCollider.motorTorque = 0;
                RRWheelCollider.motorTorque = 0;
            }
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
            if (gearState == GearState.Park || gearState == GearState.Neutral)
            {
                BrakeInput = 1.0f; // Ensure full braking force is applied
            }
            else
            {
                BrakeInput = Mathf.Clamp01(BrakeInput);
            }
            
                FLWheelCollider.brakeTorque = BrakeInput * BrakePower;
                FRWheelCollider.brakeTorque = BrakeInput * BrakePower;
                RLWheelCollider.brakeTorque = BrakeInput * BrakePower;
                RRWheelCollider.brakeTorque = BrakeInput * BrakePower;
            
            //else
            //{
            //    FLWheelCollider.brakeTorque = 0;
            //    FRWheelCollider.brakeTorque = 0;
            //    RLWheelCollider.brakeTorque = 0;
            //    RRWheelCollider.brakeTorque = 0;
            //}
        }

        void UpdateWheel()
        {
            UpdateWheelMesh(FLWheelCollider, FLWheelMesh);
            UpdateWheelMesh(FRWheelCollider, FRWheelMesh);
            UpdateWheelMesh(RLWheelCollider, RLWheelMesh);
            UpdateWheelMesh(RRWheelCollider, RRWheelMesh);
        }

        void UpdateWheelMesh(WheelCollider wheelCollider, MeshRenderer wheelMesh)
        {
            Quaternion q;
            Vector3 p;
            wheelCollider.GetWorldPose(out p, out q);
            wheelMesh.transform.position = p;
            wheelMesh.transform.rotation = q;
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