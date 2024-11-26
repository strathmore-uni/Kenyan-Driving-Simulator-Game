using UnityEngine;
using UnityEngine.UI; // For UI elements like Text or TMP_Text
using TMPro;

public class CarController : MonoBehaviour
{
    // Car settings
    public float acceleration = 20f;
    public float maxSpeed = 150f;
    public float reverseSpeed = 10f;
    public float brakeForce = 10f;
    public float steeringSpeed = 2f;
    public Rigidbody rb; // Rigidbody to move the car

    // Wheel Colliders
    public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheel Meshes
    public Transform frontLeftWheelMesh, frontRightWheelMesh;
    public Transform rearLeftWheelMesh, rearRightWheelMesh;

    // UI references
    public TMP_Text speedometerText; // To display speed
    public TMP_Text gearIndicatorText; // To display gear
    public Button buttonP, buttonN, buttonR, buttonD; // Buttons for gear shift
    public Button gasPedalButton, brakePedalButton; // Gas and Brake pedals

    // Variables to track car status
    private float currentSpeed = 0f;
    private string currentGear = "P"; // Default gear is Park
    private float steeringInput = 0f;

    // Center of mass adjustment
    public Transform carCenterOfMass;

    void Start()
    {
        // Initialize gear shift buttons
        buttonP.onClick.AddListener(() => SetGear("P"));
        buttonN.onClick.AddListener(() => SetGear("N"));
        buttonR.onClick.AddListener(() => SetGear("R"));
        buttonD.onClick.AddListener(() => SetGear("D"));

        // Initialize pedal buttons
        gasPedalButton.onClick.AddListener(Accelerate);
        brakePedalButton.onClick.AddListener(Brake);

       // Ensure the center of mass is set to a stable position
    if (carCenterOfMass != null)
    {
        rb.centerOfMass = carCenterOfMass.localPosition;
    }
    else
    {
        // Set it to a default value if not assigned
        rb.centerOfMass = new Vector3(0, -0.5f, 0);  // Adjust the values as needed
    }
    }

    void Update()
    {
        // Handle steering input (touch or tilt)
        steeringInput = Input.GetAxis("Horizontal"); // Use touchscreen input for mobile steering

        // Update the steering angle of the car
        float steeringAngle = steeringInput * steeringSpeed;
        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;

        // Update the wheel meshes to match the wheel colliders
        UpdateWheelMeshes();

        // Update the speedometer display
        UpdateSpeedometer();
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

        // Apply stabilization force to reduce any unwanted swinging or wobbling
        StabilizeCar();
    }
 
    private void StabilizeCar()
    {
        // Apply a force to stabilize the car's rotation if needed
        if (Mathf.Abs(rb.angularVelocity.y) > 0.5f) // Adjust threshold as needed
        {
            Vector3 antiSwingForce = Vector3.up * -rb.angularVelocity.y * 100f; // Counteract angular velocity around the Y-axis
            rb.AddTorque(antiSwingForce);
        }
    }


    public void SetGear(string gear)
    {
        currentGear = gear; // Set the current gear
        UpdateGearIndicator(); // Update the gear UI
        StopCarIfNeeded(); // Stop the car if in Park or Neutral
    }

    private void UpdateGearIndicator()
    {
        // Update the gear indicator on the UI
        if (gearIndicatorText != null)
        {
            gearIndicatorText.text = "G: " + currentGear;
        }
    }

    private void StopCarIfNeeded()
    {
        // If in Park or Neutral, stop the car by setting velocity and motor torque to zero
        if (currentGear == "P" || currentGear == "N")
        {
            StopCar();
            // Ensure wheel colliders don't apply any force when in Park or Neutral
            frontLeftWheelCollider.motorTorque = 0f;
            frontRightWheelCollider.motorTorque = 0f;
            rearLeftWheelCollider.motorTorque = 0f;
            rearRightWheelCollider.motorTorque = 0f;
        }
    }


    private void StopCar()
    {
        // Set velocity to zero to stop the car immediately
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; // Stops rotation too
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
        rb.velocity = Vector3.zero; // Stop movement
        rb.angularVelocity = Vector3.zero; // Stop rotation as well
        currentSpeed = 0f; // Ensure speed is set to zero
    }


    private void MoveForward()
    {
        // Apply forward movement based on speed
        rb.velocity = transform.forward * currentSpeed;

        // Apply wheel force to wheel colliders for forward movement
        frontLeftWheelCollider.motorTorque = currentSpeed * 100;
        frontRightWheelCollider.motorTorque = currentSpeed * 100;
    }

    private void MoveBackward()
    {
        // Apply reverse movement based on speed
        rb.velocity = transform.forward * currentSpeed;

        // Apply wheel force to wheel colliders for reverse movement
        frontLeftWheelCollider.motorTorque = currentSpeed * 100;
        frontRightWheelCollider.motorTorque = currentSpeed * 100;
    }

    private void UpdateWheelMeshes()
    {
        // Update wheel mesh positions based on wheel colliders' positions
        UpdateWheelMesh(frontLeftWheelCollider, frontLeftWheelMesh);
        UpdateWheelMesh(frontRightWheelCollider, frontRightWheelMesh);
        UpdateWheelMesh(rearLeftWheelCollider, rearLeftWheelMesh);
        UpdateWheelMesh(rearRightWheelCollider, rearRightWheelMesh);
    }

    private void UpdateWheelMesh(WheelCollider collider, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);

        mesh.position = pos;
        mesh.rotation = rot;
    }

    private void UpdateSpeedometer()
    {
        // Update the speedometer with the current speed
        if (speedometerText != null)
        {
            speedometerText.text = "S: " + Mathf.Round(currentSpeed) + " km/h";
        }
    }
}
