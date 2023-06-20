using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.Player
{
    public class MovementWithMouse : IMover
    {
        private Vector3 firstTouch, lastTouch;

        private bool isTouched;

        public float speed;

        private PlayerController playerController;
        private float movementSpeed = 8;
        private readonly float maxSpeed = 8;
        private readonly float normalSpeed = 2;
        private readonly float minSpeed = 1;
        private Rigidbody myRb;

        private bool input;

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
                if (lastTouch == firstTouch)
                {
                    return; //to prevent move forward when there is a input without swiping
                }

                input = true;
                var step = 1500f * Time.deltaTime;
                playerController.transform.rotation =
                    Quaternion.RotateTowards(playerController.transform.rotation, CalculateRotation(), step);
                playerController.transform.Translate(Vector3.forward * (Time.deltaTime * movementSpeed));
            }

            if (Input.GetMouseButtonUp(0))
            {
                input = false;
            }
        }

        public bool HasInput()
        {
            return input;
        }

        public void IncreaseMovementSpeed(bool isIncrease, bool isMinSpeed)
        {
            if (isMinSpeed)
            {
                movementSpeed = minSpeed;
            }
            else
            {
                movementSpeed = isIncrease ? maxSpeed : normalSpeed;
            }
        }

        public void IsStopped(bool isStopped)
        {
            if (isStopped)
            {
                movementSpeed = 0;
            }
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

        public void SetMinSpeed()
        {
            movementSpeed = minSpeed;
        }
    }
}