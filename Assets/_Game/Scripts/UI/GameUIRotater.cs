using UnityEngine;

namespace _Game.Scripts.UI
{
    public class GameUIRotater : MonoBehaviour
    {
        private Transform cam;

        private void Start()
        {
            cam = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + cam.forward);
        }
    }
}