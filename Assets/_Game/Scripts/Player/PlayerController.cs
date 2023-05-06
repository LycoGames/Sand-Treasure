using System;
using _Game.Scripts.Enums;
using _Game.Scripts.Interfaces;
using _Game.Scripts.Vehicle;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        //private IMover IMover;
        [SerializeField] private Joystick joystick;
        [SerializeField] private WheelsController wheelsController;

        private void Awake()
        {
            //IMover = new MovementWithMouse(this);
        }

        private void Update()
        {
           // MovementState();
        }

        private void FixedUpdate()
        {
            wheelsController.SetInput(joystick.Horizontal,joystick.Vertical);
        }

        private void MovementState()
        {
            //IMover.Movement();
            //moving anim
        }
    }
}