using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleInputNamespace;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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

       
        public Button buttonP, buttonN, buttonR, buttonD; // Buttons for gear shift
        public Button gasPedalButton, brakePedalButton; // Gas and Brake pedals

        // Motor Power Inputs
        public float MotorPower, SteeringPower, BrakePower;

        // Speed of vehicle
        private float speed;
        public float maxSpeed;
        private float speedClamped;
        private string currentGear = "P";
        private float currentSpeed;
        public float acceleration = 20f;

        // Steering curve
        public AnimationCurve SteeringCurve;

        // Steering sensitivity
        //public float steeringSensitivity = 0.5f;

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
        



        public float[] gearRatios;
        public float differentialRatio;
        private float currentTorque;
        private float clutch;
        private float wheelRPM;
        public AnimationCurve hpToRPMCurve;
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

        public float steeringSensitivity = 1.0f; // Steering sensitivity
        public float accelerationSensitivity = 1.0f; // Pedal sensitivity

        private float steeringInput;
        private float accelerationInput;
        public float accelerationPower = 500f;

      

        private Light brakeLight;
        private Light reverseLight;

        void Start()
        {
            RB.centerOfMass = CenterOfMass.transform.localPosition;
            RB = GetComponent<Rigidbody>();
            Cursor.visible = true;
            RB.centerOfMass = new Vector3(0, -0.9f, 0);  // Adjust the Y-value lower to make the car more stable.

            // Initialize gear shift buttons
            buttonP.onClick.AddListener(() => SetGear("P"));
            buttonN.onClick.AddListener(() => SetGear("N"));
            buttonR.onClick.AddListener(() => SetGear("R"));
            buttonD.onClick.AddListener(() => SetGear("D"));

            // Initialize pedal buttons
            gasPedalButton.onClick.AddListener(Accelerate);
            brakePedalButton.onClick.AddListener(Brake);
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
            gearText.text = (currentGear == "N") ? "N" : (currentGear + 1).ToString();



            speedKMH = RB.velocity.magnitude * 3.6f;
            speedClamped = Mathf.Lerp(speedClamped, speedKMH, Time.deltaTime);

            // Update interior speedometer


            
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

            // Get the player's input for steering and pedals (assuming input axes for steering and acceleration)
            steeringInput = SimpleInput.GetAxis("Horizontal"); // Typically the 'A/D' or arrow keys
            accelerationInput = SimpleInput.GetAxis("Vertical"); // Typically the 'W/S' or up/down arrows

            // Adjust the steering based on the sensitivity
            float steeringAngle = steeringInput * steeringSensitivity;
            float acceleration = accelerationInput * accelerationSensitivity;

            // Apply these values to the car's steering and acceleration logic

           
        }




        void Move(float direction)
        {
            // Example of moving the car
            transform.Translate(Vector3.forward * speed * direction * Time.deltaTime);
        }

        public void ApplySteering(float steerAngle)
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
            // Control car movement based on gear
            if (currentGear == "D")
            {
                MoveForward();
            }
            else if (currentGear == "R")
            {
                MoveBackward();
            }
        }


        public void SetGear(string gear)
        {
            currentGear = gear; // Set the current gear
            UpdateGearIndicator(); // Update the gear UI
            StopCarIfNeeded(); // Stop the car if in Park or Neutral
        }
        private void UpdateSpeedometer()

        {
            // Update the speedometer with the current speed
            if (rpmText != null)
            {
                rpmText.text = "S: " + Mathf.Round(currentSpeed) + " km/h";
            }
        }

        private void UpdateGearIndicator()
        {
            // Update the gear indicator on the UI
            if (gearText != null)
            {
                gearText.text = "G: " + currentGear;
            }
        }

        private void StopCarIfNeeded()
        {
            // If in Park or Neutral, stop the car by setting velocity and motor torque to zero
            if (currentGear == "P" || currentGear == "N")
            {
                StopCar();
                // Ensure wheel colliders don't apply any force when in Park or Neutral
                FLWheelCollider.motorTorque = 0f;
                FRWheelCollider.motorTorque = 0f;
                RLWheelCollider.motorTorque = 0f;
                RRWheelCollider.motorTorque = 0f;
            }
        }


        private void StopCar()
        {
            // Set velocity to zero to stop the car immediately
            RB.velocity = Vector3.zero;
            RB.angularVelocity = Vector3.zero; // Stops rotation too
            currentSpeed = 0f;
        }

        public void Accelerate()
        {
            if (currentGear == "D")
            {
                // Increase speed when gas pedal is pressed
                currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, 0f, maxSpeed);
            }
            else if (currentGear == "R")
            {
                // Increase reverse speed when gas pedal is pressed in reverse gear
                currentSpeed = Mathf.Clamp(currentSpeed - acceleration * Time.deltaTime, -reverseSpeed, 0f);
            }
        }

        public void Brake()
        {
            // Stop the car instantly
            RB.velocity = Vector3.zero; // Stop movement
            RB.angularVelocity = Vector3.zero; // Stop rotation as well
            currentSpeed = 0f; // Ensure speed is set to zero
        }


        private void MoveForward()
        {
            // Apply forward movement based on speed
            RB.velocity = transform.forward * currentSpeed;

            // Apply wheel force to wheel colliders for forward movement
            FLWheelCollider.motorTorque = currentSpeed * 100;
            FRWheelCollider.motorTorque = currentSpeed * 100;
        }

        private void MoveBackward()
        {
            // Apply reverse movement based on speed
            RB.velocity = transform.forward * currentSpeed;

            // Apply wheel force to wheel colliders for reverse movement
            FLWheelCollider.motorTorque = currentSpeed * 100;
            FRWheelCollider.motorTorque = currentSpeed * 100;
        }

        
        public float GetSpeedRatio()
        {
            var gas = Mathf.Clamp(Mathf.Abs(FuelInput), 0.5f, 1f);
            return RPM * gas / redLine;
        }

        public void ApplyMotor()
        {
           
            if (currentGear == "D")
            {
                RLWheelCollider.motorTorque = FuelInput * MotorPower;
                RRWheelCollider.motorTorque = FuelInput * MotorPower;
            }
            else if (currentGear == "R")
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
        

        void ApplyBrakes()
        {
            // Apply brakes to all wheels
            FLWheelCollider.brakeTorque = BrakeInput * BrakePower;
            FRWheelCollider.brakeTorque = BrakeInput * BrakePower;
            RLWheelCollider.brakeTorque = BrakeInput * BrakePower;
            RRWheelCollider.brakeTorque = BrakeInput * BrakePower;

           
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


        
    }
}