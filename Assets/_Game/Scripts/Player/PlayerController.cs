using _Game.Scripts.Enums;
using _Game.Scripts.Interfaces;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private IMover IMover;

        private void Awake()
        {
            IMover = new MovementWithMouse(this);
            
        }

        private void Update()
        {
            MovementState();
        }

        private void MovementState()
        {
            IMover.Movement();
            //moving anim
        }
    }
}