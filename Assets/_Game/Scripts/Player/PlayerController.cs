using _Game.Scripts.Enums;
using _Game.Scripts.Interfaces;
using _Game.Scripts.StatSystem;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private StateController stateController;

        private IMover IMover;
        private bool isInSellZone = false;
        [SerializeField] private Stats stats;
        
        private void Awake()
        {
            IMover = new MovementWithMouse(this);
            Actions.onCollisionSellZone += () => isInSellZone = !isInSellZone;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                stats.UpgradeStat(Stat.StackCapacity);
                print(stats.GetStat(Stat.StackCapacity)+" "+stats.GetStatLevel(Stat.StackCapacity));
            }
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