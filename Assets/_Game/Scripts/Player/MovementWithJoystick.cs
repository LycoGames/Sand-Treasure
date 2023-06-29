using System;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Vehicle;
using Joystick_Pack.Scripts.Base;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class MovementWithJoystick : IMover
    {
        private Joystick joystick;
        private WheelsController wheelsController;
        private float steerSense;
        private PlayerController playerController;

        public MovementWithJoystick(PlayerController playerController, WheelsController wheelsController,
            Joystick joystick, float steerSense) // set player controller in here.
        {
            this.wheelsController = wheelsController;
            this.joystick = joystick;
            this.steerSense = steerSense;
            this.playerController = playerController;
        }

        public void Movement()
        {
            Vector3 target = new Vector3(joystick.Horizontal, 0, joystick.Vertical) +
                             playerController.transform.position;
            Vector3 relative = playerController.transform.InverseTransformPoint(target);
            relative /= relative.magnitude;
            float steerAngleAI = (relative.x / relative.magnitude) * steerSense;
            wheelsController.SetInput(steerAngleAI, Math.Abs(joystick.Horizontal) + Math.Abs(joystick.Vertical));
        }

        public bool HasInput()
        {
            return joystick.Vertical + joystick.Horizontal!=0;
        }

        public void IncreaseMovementSpeed(bool isIncrease, bool isMinSpeed)
        {
            throw new NotImplementedException();
        }

        public void IsStopped(bool isStopped)
        {
            throw new NotImplementedException();
        }

        public void UpdateSpeed(float value)
        {
            throw new NotImplementedException();
        }
    }
}