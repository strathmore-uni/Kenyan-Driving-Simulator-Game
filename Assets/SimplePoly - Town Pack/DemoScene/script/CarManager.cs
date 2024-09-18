using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleInputNamespace;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RVP;
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
        bool isAccelerating = false;
        public float neutralDeceleration = 0f;
        public float slowSpeed = 5f;     // Speed when moving slowly when the gas pedal is not pressed
        public float acceleration = 5f;  // Acceleration rate
        public float deceleration = 5f;  // Deceleration rate
        public float brakeForce = 5.0f;
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

            // Ensure lights are off initially

        }
        public void ParkGear()
        {
            gearState = GearState.Park;
            currentGear = 0;
            // Stop the car
            RB.velocity = Vector3.zero;
            ApplyBrakes(); // Ensure brakes are applied when parked
        }

        public void NeutralGear()
        {
            gearState = GearState.Neutral;
            currentGear = 0;
            RB.velocity = Vector3.zero;         // Stop linear movement
            RB.angularVelocity = Vector3.zero;  // Stop rotational movement
            ApplyBrakes(); // Optionally apply brakes
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
            /*reverseLight.enabled = true;*/ // Ensure reverse light is on when in reverse
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
            speedKMH = Mathf.Lerp(speedKMH, RB.velocity.magnitude * 3.6f, Time.deltaTime * 10f);


            // Update interior speedometer


            CheckInputs();
            ApplyMotor();
            //ApplySteering();
            UpdateWheel();
            ApplyBrakes();
            Brake(BrakeInput);
            HandleMovement();
            HandleBraking();
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
            // Add some debug logs to see if the input values are being set correctly
            Debug.Log("FuelInput: " + fuelInput);
            Debug.Log("BrakeInput: " + brakeInput);
        }

        void FixedUpdate()
        {
            ActivateLights();
            animatorTurnAngle = Mathf.Lerp(animatorTurnAngle, -SimpleInput.GetAxis("Horizontal"), 28f * Time.deltaTime);


            float fuelInput;
            float brakeInput;
            CheckInputs(out fuelInput, out brakeInput);

            if (gearState == GearState.Park || gearState == GearState.Neutral)
            {
                // When in Park or Neutral, apply brakes and do not apply motor torque
                ApplyBrakes();
                return; // Exit FixedUpdate early
            }

            // Apply braking only if brake input is detected
            if (brakeInput > 0.0f)
            {
                ApplyBrakes(); // Apply brakes when brake input is detected
            }
            else
            {
                // Adjust acceleration and deceleration
                if (gearState == GearState.Drive && SimpleInput.GetAxis("GasPedal") > 0)
                {
                    Accelerate(fuelInput);
                }
                else if (gearState == GearState.Reverse && SimpleInput.GetAxis("GasPedal") < 0)
                {
                    Reverse(fuelInput);
                }
                else
                {
                    // If no brake input and no fuel input, slow down the car gradually
                    SlowDown();
                }
            }
            // Add a small amount of drag to the car's movement
            if (RB.velocity.magnitude > 0.1f)
            {
                RB.velocity *= 0.99f;
            }


            ApplyMotor();
        }


        public void Accelerate(float input)
        {
            if (gearState == GearState.Drive)
            {
                RB.AddForce(transform.forward * accelerationForce * input, ForceMode.Acceleration);
                if (RB.velocity.magnitude > forwardSpeed)
                {
                    RB.velocity = Vector3.ClampMagnitude(RB.velocity, forwardSpeed);
                }
            }
        }


        public void Brake(float brakeForce)
        {
            // Apply brake force by directly modifying the velocity
            if (RB.velocity.magnitude > 0)
            {
                // Gradually reduce the velocity to zero based on brakeForce
                RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, brakeForce * Time.deltaTime);

            }
        }


        public void Reverse(float input)
        {
            if (gearState == GearState.Reverse)
            {
                // Apply reverse force
                RB.AddForce(-transform.forward * accelerationForce * input, ForceMode.Acceleration);

                // Limit the reverse speed
                if (RB.velocity.magnitude > reverseSpeed)
                {
                    RB.velocity = Vector3.ClampMagnitude(RB.velocity, reverseSpeed);
                }
            }
        }

        void Neutral()
        {
            // Optionally, apply friction or damping to stop the car
            RB.velocity = Vector3.zero;
            RB.angularVelocity = Vector3.zero;
        }


        void SlowDown()
        {
            // Set a minimum speed threshold (in meters per second)
            float minimumSpeed = 2.0f; // Adjust this value for your desired slow speed

            // Check the current speed magnitude
            float currentSpeed = RB.velocity.magnitude;

            // If the car's current speed is above the minimum speed, apply braking force
            if (currentSpeed > minimumSpeed)
            {
                // Reduce the car's speed gradually
                RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, 0.1f); // Adjust the 0.1f value for the rate of slowdown

                // Apply a gentle braking force to slow down the car
                RB.AddForce(-RB.velocity.normalized * brakingForce * 0.05f, ForceMode.Acceleration); // Reduce force
            }
            else
            {
                // If the car's speed is lower than the minimum threshold, maintain the minimum speed
                RB.velocity = RB.velocity.normalized * minimumSpeed;
            }
            
        }
        public void Idle()
        {
            // Implement idling logic here
            Debug.Log("Idling");
        }

        private void HandleMovement()
        {
            // Handle movement based on gear state
            if (gearState == GearState.Park)
            {
                // No movement in Park
                RB.velocity = Vector3.zero;
            }
            else if (gearState == GearState.Reverse)
            {
                // Move backwards if the gas pedal is pressed
                if (gasPedal.isPressed)
                {
                    RB.velocity = Vector3.Lerp(RB.velocity, -transform.forward * reverseSpeed, acceleration * Time.deltaTime);
                }
                else
                {
                    // Gradually move forward if the gas pedal is not pressed
                    RB.velocity = Vector3.Lerp(RB.velocity, transform.forward * slowSpeed, deceleration * Time.deltaTime);
                }
            }
            else if (gearState == GearState.Neutral)
            {
                // No movement in Neutral, apply deceleration to stop
                RB.velocity = Vector3.Lerp(RB.velocity, Vector3.zero, neutralDeceleration * Time.deltaTime);
            }
            else if (gearState == GearState.Drive)
            {
                // Move forwards if in Drive
                if (gasPedal.isPressed)
                {
                    RB.velocity = Vector3.Lerp(RB.velocity, transform.forward * speed, acceleration * Time.deltaTime);
                }
                else
                {
                    // Gradually reduce speed if the gas pedal is not pressed
                    RB.velocity = Vector3.Lerp(RB.velocity, transform.forward * slowSpeed, deceleration * Time.deltaTime);
                }
            }

            // Apply braking
            if (brakePedal.isPressed)
            {
                Brake(1.0f); // Full braking force
            }
        }


        void HandleBraking()
        {
            if (BrakeInput > 0)
            {
                // Immediately set the car's velocity to zero
                RB.velocity = Vector3.zero;

                // Apply angular damping to stabilize the car's rotation
                float angularDamping = 5.0f;
                RB.AddTorque(-RB.angularVelocity * angularDamping, ForceMode.Acceleration);
            }
        }

        void ResetCarMovement()
        {
            // Reset car velocity to zero when switching gears
            RB.velocity = Vector3.zero;
        }

        //float doubleClickTime = 0.5f; // Time in seconds to consider a double click
        //float lastBrakePressTime = 0f;

        void CheckInputs()
        {
            FuelInput = SimpleInput.GetAxis("Vertical");
            if (gasPedal.isPressed)
            {
                if (gearState == GearState.Drive)
                {
                    FuelInput = Mathf.Clamp(gasPedal.dampenPress, 0, 1);
                }
                else if (gearState == GearState.Reverse)
                {
                    FuelInput = Mathf.Clamp(gasPedal.dampenPress, -1, 0);
                }



                //// Check for double click
                //float currentTime = Time.time;
                //if (currentTime - lastBrakePressTime < doubleClickTime)
                //{
                //    // Double click detected, apply emergency brake
                //    FuelInput = -1f; // Set FuelInput to maximum braking value
                //}
                //lastBrakePressTime = currentTime;
            }
            if (brakePedal.isPressed)
            {
                // Apply brakes based on brake pedal input
                BrakeInput = 1.0f; // Full braking force
            }
            else
            {
                BrakeInput = 0.0f; // No braking force
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

            if (gearState == GearState.Neutral)
            {
                clutch = 0;
                if (Mathf.Abs(FuelInput) > 0)
                {
                    gearState = GearState.Drive; // Or Reverse, depending on your logic
                }
            }
            else
            {
                clutch = Input.acceleration.y > 0.5f ? 0 : Mathf.Lerp(clutch, 1, Time.deltaTime);
            }

            // Apply brake force based on fuel input and movement direction
            if (gearState == GearState.Drive && RB.velocity.magnitude > 0)
            {
                if (FuelInput < 0)
                {
                    BrakeInput = Mathf.Abs(FuelInput); // Apply brakes if moving forward and trying to reverse
                }
            }
            else if (gearState == GearState.Reverse && RB.velocity.magnitude > 0)
            {
                if (FuelInput > 0)
                {
                    BrakeInput = Mathf.Abs(FuelInput); // Apply brakes if moving backward and trying to accelerate
                }
            }
        }
        public float GetSpeedRatio()
        {
            var gas = Mathf.Clamp(Mathf.Abs(FuelInput), 0.5f, 1f);
            return RPM * gas / redLine;
        }

        public void ApplyMotor()
        {
            if (gearState == GearState.Drive)
            {
                RLWheelCollider.motorTorque = FuelInput * MotorPower;
                RRWheelCollider.motorTorque = FuelInput * MotorPower;
                RB.AddForce(transform.forward * accelerationForce * FuelInput, ForceMode.Acceleration);
            }
            else if (gearState == GearState.Reverse)
            {
                RLWheelCollider.motorTorque = -FuelInput * MotorPower;
                RRWheelCollider.motorTorque = -FuelInput * MotorPower;
                RB.AddForce(-transform.forward * accelerationForce * FuelInput, ForceMode.Acceleration);
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

            // Handle Neutral Gear
            if (RPM < idleRPM + 200 && FuelInput == 0 && currentGear == 0)
            {
                gearState = GearState.Neutral;
            }

            // Gear Changing Logic
            if (gearState == GearState.Running && clutch > 0)
            {
                if (RPM > increaseGearRPM && currentGear < gearRatios.Length - 1)
                {
                    StartCoroutine(ChangeGear(1));
                }
                else if (RPM < decreaseGearRPM && currentGear > 0)
                {
                    StartCoroutine(ChangeGear(-1));
                }
            }

            // Torque Calculation
            if (isEngineRunning > 0)
            {
                if (clutch < 0.5f)
                {
                    torque = MotorPower;
                }
                else
                {
                    torque = 0; // No torque applied if clutch is not engaged properly
                }
            }

            return torque;
        }


        void ApplyBrakes()
        {
            
                BrakeInput = Mathf.Clamp01(BrakeInput);
                FLWheelCollider.brakeTorque = BrakeInput * BrakePower;
                FRWheelCollider.brakeTorque = BrakeInput * BrakePower;
                RLWheelCollider.brakeTorque = BrakeInput * BrakePower;
                RRWheelCollider.brakeTorque = BrakeInput * BrakePower;
            

            // Apply brakes to all wheels
            //FLWheelCollider.brakeTorque = BrakeInput * BrakePower;
            //FRWheelCollider.brakeTorque = BrakeInput * BrakePower;
            //RLWheelCollider.brakeTorque = BrakeInput * BrakePower;
            //RRWheelCollider.brakeTorque = BrakeInput * BrakePower;

            // Turn on the brake light when braking

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
            Debug.Log($"Gear Change Requested: {gearChange}, Current Gear: {currentGear}, RPM: {RPM}");

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