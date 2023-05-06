using System;
using UnityEngine;

namespace _Game.Scripts.Vehicle
{
    public class WheelsController : MonoBehaviour
    {
        [SerializeField] private float maxspeed;
        [SerializeField] private float MaxSteeringAngle;
        [SerializeField] private Rigidbody myRb;
        [SerializeField] private float motorForce;
        [SerializeField] private float brakeForce;
        [SerializeField] private float downForce;
        [SerializeField] private Vector3 centerOfMass;

        [Header("Wheel Colliders")] [Space] 
        [SerializeField] private WheelCollider frontleftWheelCollider;
        [SerializeField] private WheelCollider frontrightWheelCollider;
        [SerializeField] private WheelCollider rearleftWheelCollider;
        [SerializeField] private WheelCollider rearrightWheelCollider;

        [Space] [Header("Wheel Transforms")] [Space] 
        [SerializeField] private Transform frontleftWheelTransform;
        [SerializeField] private Transform frontrightWheelTransform;
        [SerializeField] private Transform rearleftWheelTransform;
        [SerializeField] private Transform rearrightWheelTransform;

        private float currentMotorForce;
        private float currentInput;
        private float steerAngle;
        private float currentSteerAngle;
        private int speedlimiting;
        private float horizontalInput;
        private float verticalInput;
        private float currentBrakeForce;

        private void Start()
        {
            myRb.centerOfMass = centerOfMass;
        }

        private void FixedUpdate()
        {
            ApplyDownForce();
            HandleMotor();
            ApplyBreaking();
            HandleSteering();
            UpdateWheels();
        }

        public void SetInput(float horizontalInput, float verticalInput)
        {
            this.horizontalInput = horizontalInput;
            this.verticalInput = verticalInput;
        }

        private void ApplyDownForce()
        {
            myRb.AddForce(-transform.up * downForce);
        }

        private bool isReachMaxSpeed()
        {
            return myRb.velocity.magnitude > maxspeed;
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
            currentInput = verticalInput > horizontalInput ? verticalInput : horizontalInput;
            currentMotorForce = isReachMaxSpeed() ?  0:verticalInput * motorForce;
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
            currentBrakeForce = brakeForce * (currentInput==0?1:0);
            rearleftWheelCollider.brakeTorque = currentBrakeForce;
            rearrightWheelCollider.brakeTorque = currentBrakeForce;
            frontleftWheelCollider.brakeTorque = currentBrakeForce;
            frontrightWheelCollider.brakeTorque = currentBrakeForce;
        }
    }
}