using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class WheelAnimator : MonoBehaviour
    {
        [SerializeField] private List<Transform> wheels = new List<Transform>();
        [SerializeField] private int rotateSpeed;
    
        public void AnimateWheels()
        {
            foreach (var wheel in wheels)
            {
                wheel.Rotate(rotateSpeed * Time.deltaTime, 0.0f, 0.0f);
            }
        }
    }
}