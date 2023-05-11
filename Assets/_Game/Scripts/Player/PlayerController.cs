using _Game.Scripts.Enums;
using _Game.Scripts.Interfaces;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private StateController stateController;

        private IMover IMover;
        private bool isInSellZone = false;

        private void Awake()
        {
            IMover = new MovementWithMouse(this);
            Actions.onCollisionSellZone += () => isInSellZone = !isInSellZone;
        }

        private void Update()
        {
            if (IMover.HasInput() || isInSellZone)
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
            IMover.Movement();
            //moving anim
        }
    }
}