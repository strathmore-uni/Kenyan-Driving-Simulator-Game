using UnityEngine;
using System.Collections;
using SimpleInputNamespace;

namespace RVP
{
    [DisallowMultipleComponent]
    [AddComponentMenu("RVP/Vehicle Controllers/Steering Control", 2)]

    // Class for steering vehicles
    public class InteriorSteeringController : MonoBehaviour
    {
        Transform tr;
        VehicleParent vp;
        public float steerRate = 0.1f; // The rate at which the steering wheel rotates
        float steerAmount;

        [Tooltip("Curve for limiting steer range based on speed, x-axis = speed, y-axis = multiplier")]
        public AnimationCurve steerCurve = AnimationCurve.Linear(0, 1, 30, 0.1f);
        public bool limitSteer = true;

        [Tooltip("Horizontal stretch of the steer curve")]
        public float steerCurveStretch = 1;
        public bool applyInReverse = true; // Limit steering in reverse?

        [Header("Visual")]

        public Transform[] steeredWheelMeshes; // Now using Transform instead of Suspension
        public bool rotate;
        public float maxDegreesRotation = 450f; // Maximum rotation (e.g., 450 degrees)
        public float rotationOffset = 0f;
        float steerRot;

        private Vector3 lastMousePosition;
        private bool isDragging = false;

        public float maxSteerAngle = 30f; // Maximum angle the steering wheel can turn
        public GameObject steeringWheel; // Reference to the steering wheel GameObject
        public float smoothFactor = 5f; // Factor to control the smoothness of the rotation

        void Start()
        {
            tr = transform; // Cache the transform of the steering wheel
            vp = tr.GetTopmostParentComponent<VehicleParent>(); // Get the vehicle parent component
            steerRot = rotationOffset; // Initialize the rotation with the offset
        }

        void Update()
        {
            HandleMouseInput(); // Capture mouse input for steering

            // Visual steering wheel rotation based on steerAmount
            if (rotate)
            {
                steerRot = Mathf.Lerp(steerRot, steerAmount * maxDegreesRotation + rotationOffset, steerRate * Time.deltaTime);
                tr.localRotation = Quaternion.Euler(tr.localEulerAngles.x, tr.localEulerAngles.y, -steerRot); // Apply rotation to the Z-axis without changing the position
            }
        }


        private void HandleMouseInput()
        {
            // Begin dragging
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
                isDragging = true;
            }

            // End dragging
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // Handle steering input during drag
            if (isDragging)
            {
                Vector3 mouseDelta = Input.mousePosition - lastMousePosition;

                // Calculate the steering input based on horizontal mouse movement
                steerAmount += mouseDelta.x * steerRate;
                steerAmount = Mathf.Clamp(steerAmount, -1f, 1f); // Clamping the steerAmount between -1 and 1

                lastMousePosition = Input.mousePosition;
            }
        }
        void UpdateSteeringWheel(float steerInput)
        {
            // Calculate the target angle
            float targetAngle = steerInput * maxSteerAngle;

            // Apply the rotation to the steering wheel's local Z-axis
            steeringWheel.transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
        }

    }
}
