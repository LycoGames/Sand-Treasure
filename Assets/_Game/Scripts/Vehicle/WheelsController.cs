using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Vehicle
{
    public class WheelsController : MonoBehaviour
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float maxSpeedBraking;
        [SerializeField] private float MaxSteeringAngle;
        [SerializeField] private Rigidbody myRb;
        [SerializeField] private float motorForce;
        [SerializeField] private float brakeForce;
        [SerializeField] private float downForce;
        [SerializeField] private Vector3 centerOfMass;

        [Header("Wheel Colliders")] [Space] [SerializeField]
        private WheelCollider frontleftWheelCollider;

        [SerializeField] private WheelCollider frontrightWheelCollider;
        [SerializeField] private WheelCollider rearleftWheelCollider;
        [SerializeField] private WheelCollider rearrightWheelCollider;

        [Space] [Header("Wheel Transforms")] [Space] [SerializeField]
        private Transform frontleftWheelTransform;

        [SerializeField] private Transform frontrightWheelTransform;
        [SerializeField] private Transform rearleftWheelTransform;
        [SerializeField] private Transform rearrightWheelTransform;

        private float currentMotorForce;
        private float steerAngle;
        private float currentSteerAngle;
        private int speedlimiting;
        private float horizontalInput;
        private float verticalInput;
        private float currentBrakeForce;

        private int groundedCount;
        private float groundPercent;
        private float airPercent;

        private void Start()
        {
            myRb.centerOfMass = centerOfMass;
        }

        private void FixedUpdate()
        {
            CheckGrounded();
            HandleMotor();
            ApplyBreaking();
            HandleSteering();
            UpdateWheels();
        }

        public void SetInput(float horizontalInput, float verticalInput)
        {
            this.horizontalInput = Math.Clamp(horizontalInput,-1,1);
            this.verticalInput = Math.Clamp(verticalInput,-1,1);
        }
        private void CheckGrounded()
        {
            groundedCount = 0;
            if (frontleftWheelCollider.isGrounded && frontleftWheelCollider.GetGroundHit(out WheelHit hit))
                groundedCount++;
            if (frontleftWheelCollider.isGrounded && frontleftWheelCollider.GetGroundHit(out hit))
                groundedCount++;
            if (rearleftWheelCollider.isGrounded && rearleftWheelCollider.GetGroundHit(out hit))
                groundedCount++;
            if (rearrightWheelCollider.isGrounded && rearrightWheelCollider.GetGroundHit(out hit))
                groundedCount++;

            // calculate how grounded and airborne we are
            groundPercent = (float) groundedCount / 4.0f;
            airPercent = 1 - groundPercent;
            if (airPercent >= 1)
            {
                myRb.velocity += Physics.gravity * Time.fixedDeltaTime * downForce;
            }
        }

        private bool isReachMaxSpeed()
        {
            return myRb.velocity.magnitude > maxSpeed;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(frontleftWheelCollider, frontleftWheelTransform);
            UpdateSingleWheel(frontrightWheelCollider, frontrightWheelTransform);
            UpdateSingleWheel(rearleftWheelCollider, rearleftWheelTransform);
            UpdateSingleWheel(rearrightWheelCollider, rearrightWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider WheelCollider, Transform WheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            WheelCollider.GetWorldPose(out pos, out rot);
            WheelTransform.rotation = rot;
            WheelTransform.position = pos;
        }

        private void HandleMotor()
        {
            currentMotorForce = isReachMaxSpeed() ? 0 : verticalInput * motorForce;
            rearleftWheelCollider.motorTorque = currentMotorForce;
            rearrightWheelCollider.motorTorque = currentMotorForce;
            frontleftWheelCollider.motorTorque = currentMotorForce;
            frontrightWheelCollider.motorTorque = currentMotorForce;
        }

        private void HandleSteering()
        {
            currentSteerAngle = MaxSteeringAngle * horizontalInput;
            frontleftWheelCollider.steerAngle = currentSteerAngle;
            frontrightWheelCollider.steerAngle = currentSteerAngle;
        }

        private void ApplyBreaking()
        {
            if (isReachMaxSpeed()) currentBrakeForce = maxSpeedBraking*(myRb.velocity.magnitude-maxSpeed);
            else if (verticalInput == 0) currentBrakeForce = brakeForce;
            else currentBrakeForce = 0;
            rearleftWheelCollider.brakeTorque = currentBrakeForce;
            rearrightWheelCollider.brakeTorque = currentBrakeForce;
            frontleftWheelCollider.brakeTorque = currentBrakeForce;
            frontrightWheelCollider.brakeTorque = currentBrakeForce;
        }
    }
}