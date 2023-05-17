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
        [SerializeField] private Stats stats;
        [SerializeField] private WheelAnimator wheelAnimator;


        //   [SerializeField] private WheelsController wheelsController;
        //   [SerializeField] private Joystick joystick;
        //   [SerializeField] private float steerSense;

        private bool isInSellZone = false;
        private IMover Imover;

        private void Awake()
        {
            Actions.onCollisionSellZone += () => isInSellZone = !isInSellZone;
            Imover = new MovementWithMouse(this);
        }

        private void Update()
        {
            if (Imover.HasInput())
            {
                if (stateController.CurrentState == stateController.IdleState)
                {
                    stateController.ChangeState(stateController.DigState);
                }

                wheelAnimator.AnimateWheels();
            }
            else
            {
                if (stateController.CurrentState == stateController.DigState )
                {
                    stateController.ChangeState(stateController.IdleState);
                }
            }

            Movement();
        }

        private void Movement()
        {
            Imover.Movement();
            //moving anim
        }
    }
}