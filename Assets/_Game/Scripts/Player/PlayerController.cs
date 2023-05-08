using _Game.Scripts.Enums;
using _Game.Scripts.Interfaces;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private StateController stateController;

        private IMover IMover;

        private void Awake()
        {
            IMover = new MovementWithMouse(this);
        }

        private void Update()
        {
            if (IMover.HasInput())
            {
                if (stateController.CurrentState == stateController.DigState)
                {
                    stateController.ChangeState(stateController.MovementState);
                }
            }
            else
            {
                if (stateController.CurrentState == stateController.MovementState)
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