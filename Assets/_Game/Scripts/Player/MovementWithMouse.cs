using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Interfaces;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.Player
{
    public class MovementWithMouse : IMover
    {
        private Vector3 firstTouch, lastTouch;

        private bool isTouched;

        public float speed;

        //  private PlayerAnimatorController playerAnimatorController;
        private PlayerController playerController;
        private float movementSpeed=10f;

        public MovementWithMouse(PlayerController playerController) // set player controller in here.
        {
            this.playerController = playerController;
        }


        public void Movement()
        {
            if (IsPointerOverUIElement()) return;
            if (Input.GetMouseButtonDown(0))
            {
                firstTouch = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                lastTouch = Input.mousePosition;
                var deltaPos = (lastTouch - firstTouch).normalized;
                if (lastTouch == firstTouch) return; //to prevent move forward when there is a input without swiping
                var step = 1500f * Time.deltaTime;
                playerController.transform.rotation =
                    Quaternion.RotateTowards(playerController.transform.rotation, CalculateRotation(), step);
                playerController.transform.Translate(Vector3.forward * (Time.deltaTime * movementSpeed));
                //set run anim in here
            }
            // else set idle anim in here
        }


        private Quaternion CalculateRotation()
        {
            Vector3 direction = (lastTouch - firstTouch).normalized;
            float x = direction.x;
            float z = direction.y;
            Vector3 newDir = new Vector3(x, 0f, z);
            Quaternion temp = Quaternion.LookRotation(newDir, Vector3.up);
            return temp;
        }

        private bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }

        private void UpdateMovementSpeed(float speed)
        {
            movementSpeed = speed;
        }

        private bool IsPointerOverUIElement(IEnumerable<RaycastResult> eventSystemRaycastResults)
        {
            return eventSystemRaycastResults.Any(raycastResult =>
                raycastResult.gameObject.layer == LayerMask.NameToLayer("InGameUI"));
        }


        //Gets all event system raycast results of current mouse or touch position.
        private IEnumerable<RaycastResult> GetEventSystemRaycastResults()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults;
        }
    }
}