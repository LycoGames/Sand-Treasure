using _Game.Scripts.Interfaces;
using _Game.Scripts.Mesh;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private MeshDeformer meshDeformer;

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