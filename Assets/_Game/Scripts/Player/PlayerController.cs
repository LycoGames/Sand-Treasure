using _Game.Scripts.Enums;
using _Game.Scripts.Interfaces;
using _Game.Scripts.StatSystem;
using _Game.Scripts.Vehicle;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private StateController stateController;

        [SerializeField] private MovementWithJoystick movementWithJoystick;
        private bool isInSellZone = false;
        [SerializeField] private Stats stats;

        [SerializeField] private WheelsController wheelsController;
        [SerializeField] private Joystick joystick;
        [SerializeField] private float steerSense;

        private void Awake()
        {
            Actions.onCollisionSellZone += () => isInSellZone = !isInSellZone;
            movementWithJoystick.Setup(wheelsController,joystick,steerSense);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stats.UpgradeStat(Stat.StackCapacity);
                print(stats.GetStat(Stat.StackCapacity)+" "+stats.GetStatLevel(Stat.StackCapacity));
            }
            if (movementWithJoystick.HasInput() || isInSellZone)
            {
                if (stateController.CurrentState == stateController.DigState)
                {
                    stateController.ChangeState(stateController.IdleState);
                }
            }
            else
            {
                if (stateController.CurrentState == stateController.IdleState)
                {
                    stateController.ChangeState(stateController.DigState);
                }
            }

            Movement();
        }

        private void Movement()
        {
            movementWithJoystick.Movement();
            //moving anim
        }
    }
}