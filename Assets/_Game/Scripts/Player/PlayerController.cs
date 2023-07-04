using System;
using _Game.Scripts.Enums;
using _Game.Scripts.Interfaces;
using _Game.Scripts.States;
using _Game.Scripts.StatSystem;
using _Game.Scripts.Utils;
using _Game.Scripts.Vehicle;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private StateController stateController;
        [SerializeField] private WheelAnimator wheelAnimator;
        [SerializeField] private Stats stats;


        //   [SerializeField] private WheelsController wheelsController;
        //   [SerializeField] private Joystick joystick;
        //   [SerializeField] private float steerSense;

        private bool isInSellZone = false;
        private IMover Imover;
        public bool IsCanMove { get; set; } = true;

        private void Start()
        {
            stats.OnMovementSpeedChange += UpdateMovementSpeed;
            UpdateMovementSpeed(stats.GetStat(Stat.MovementSpeed));
        }

        private void UpdateMovementSpeed(float obj)
        {
            Imover.UpdateSpeed(obj);
        }

        private void Awake()
        {
            Actions.onCollisionSellZone += () => isInSellZone = !isInSellZone;
            Imover = new MovementWithMouse(this);
        }

        public void IncreaseMovementSpeed(bool isIncrease, bool isMinSpeed)
        {
            Imover.IncreaseMovementSpeed(isIncrease, isMinSpeed);
        }

        public void StopPlayer(bool isStopped)
        {
            Imover.IsStopped(isStopped);
        }

        private void Update()
        {
            if (!IsCanMove)
            {
                return;
            }

            if (Imover.HasInput())
            {
                // if (stateController.CurrentState == stateController.IdleState)
                // {
                //     stateController.ChangeState(stateController.DigState);
                // }
                wheelAnimator.AnimateWheels();
            }

            // else
            // {
            //     if (stateController.CurrentState == stateController.DigState )
            //     {
            //         stateController.ChangeState(stateController.IdleState);
            //     }
            // }
            Movement();
        }

        private void Movement()
        {
            Imover.Movement();
        }
    }
}