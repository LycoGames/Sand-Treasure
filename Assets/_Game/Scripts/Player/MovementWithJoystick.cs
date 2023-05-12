using System;
using _Game.Scripts.Vehicle;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class MovementWithJoystick : MonoBehaviour
    {
        private Joystick joystick;
        private WheelsController wheelsController;
        private float steerSense;
        
        public void Setup(WheelsController wheelsController,Joystick joystick,float steerSense) // set player controller in here.
        {
            this.wheelsController = wheelsController;
            this.joystick = joystick;
            this.steerSense = steerSense;
        }

        public void Movement()
        {
            Vector3 target = new Vector3(joystick.Horizontal, 0, joystick.Vertical)+transform.position;
            Vector3 relative = transform.InverseTransformPoint(target);
            relative /= relative.magnitude;
            float steerAngleAI = (relative.x / relative.magnitude) * steerSense;
            wheelsController.SetInput(steerAngleAI,Math.Abs(joystick.Horizontal)+Math.Abs(joystick.Vertical));
        }

        public bool HasInput()
        {
            return joystick.Vertical + joystick.Horizontal!=0;
        }
    }
}
