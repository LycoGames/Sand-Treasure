using System;
using _Game.Scripts.Enums;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Vehicle;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private IMover IMover;

        [SerializeField] private float steerSense=5f;
        [SerializeField] private Joystick joystick;
        [SerializeField] private WheelsController wheelsController;

        private void Awake()
        {
            //IMover = new MovementWithMouse(this);
        }

        private void Update()
        {
           // MovementState();
           Vector3 target = new Vector3(joystick.Horizontal, 0, joystick.Vertical)+transform.position;
           Vector3 relative = transform.InverseTransformPoint(target);
           relative /= relative.magnitude;
           float steerAngleAI = (relative.x / relative.magnitude) * steerSense;
           Vector3 playerRelativeToObject = target - transform.position;
           wheelsController.SetInput(steerAngleAI,Math.Abs(joystick.Horizontal)+Math.Abs(joystick.Vertical));
           //Debug.Log(transform.rotation.y);
        }
        
        private void MovementState()
        {
            //IMover.Movement();
            //moving anim
        }
    }
}