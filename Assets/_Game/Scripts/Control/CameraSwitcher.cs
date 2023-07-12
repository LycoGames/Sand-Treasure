using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Player;
using Cinemachine;
using UnityEngine;

namespace _Game.Scripts.Control
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private List<CinemachineVirtualCamera> tutorialCameraList;
        [SerializeField] private float waitSecondOnCam = 2f;

        private PlayerController playerController;

        private void Awake()
        {
            foreach (var camera in tutorialCameraList)
            {
                camera.Priority = 0;
            }
        }

        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void ActivateCam(int index)
        {
            StartCoroutine(CinematicCoroutine(index));
        }

        private IEnumerator CinematicCoroutine(int index)
        {
            tutorialCameraList[index].Priority = 5;
            playerController.IsCanMove = false;
            yield return new WaitForSeconds(waitSecondOnCam + 1f);
            tutorialCameraList[index].Priority = 0;
            yield return new WaitForSeconds(1f);
            playerController.IsCanMove = true;
        }
    }
}