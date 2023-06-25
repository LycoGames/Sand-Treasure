using System;
using System.Collections;
using System.Linq;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace _Game.Utils.LineRendererGPS
{
    public class Line : MonoBehaviour
    {
        public UnityEvent onAllPointsTraveled;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private TutorialElement[] tutorialElements;
        [SerializeField] private float speed = 30f;
        [SerializeField] private float speedMultiplier;
        private int currentIndex = 0;

        private Vector3 tempPos;
        private Transform destination;
        private WaitForSeconds lineRendererChangeDelayWaitForSeconds;
        private Coroutine coroutine;

        // private void OnEnable()
        // {
        //     SetFirstDestination();
        //     StartCoroutine(LineRendererCoroutine());
        // }

        public void SetPlayer(Transform player)
        {
            playerTransform = player;
            StartRenderer();
        }

        public void StartCoroutine()
        {
            EnableDisableRenderer(true);
            coroutine = StartCoroutine(LineRendererCoroutine());
        }

        public void StopCoroutine()
        {
            EnableDisableRenderer(false);
            StopCoroutine(coroutine);
        }

        private void StartRenderer()
        {
            SetFirstDestination();
            coroutine = StartCoroutine(LineRendererCoroutine());
        }

        private void SetFirstDestination()
        {
            if (!tutorialElements.Any()) return;

            destination = tutorialElements[currentIndex].GetDestinationTransform();
            tutorialElements[currentIndex].OnConditionComplete += GoNextIndex;
        }


        private bool CanGetNextIndex(int i)
        {
            return i + 1 < tutorialElements.Length;
        }

        private int GetNextPointIndex(int i)
        {
            return i + 1;
        }

        private IEnumerator LineRendererCoroutine()
        {
            tempPos = playerTransform.position;
            tempPos.y = 0.0f;
            while (true)
            {
                speed = CalculateSpeedByMeters(CalculateDistance(tempPos));
                while (CalculateDistance(tempPos) > 0.5f)
                {
                    lineRenderer.SetPosition(0, playerTransform.position);
                    lineRenderer.SetPosition(1, tempPos);
                    tempPos = Vector3.MoveTowards(tempPos, destination.position, speed);
                    yield return null;
                }

                tempPos = playerTransform.position;
                yield return null;
            }
        }

        private void GoNextIndex()
        {
            if (!CanGetNextIndex(currentIndex))
            {
                onAllPointsTraveled?.Invoke();
                DisableRenderer();
                return;
            }

            tutorialElements[currentIndex].OnConditionComplete -= GoNextIndex;
            SetNextIndex();
            tutorialElements[currentIndex].OnConditionComplete += GoNextIndex;
        }

        private void SetNextIndex()
        {
            currentIndex = GetNextPointIndex(currentIndex);
            destination = tutorialElements[currentIndex].GetDestinationTransform();
            tempPos = playerTransform.position;
            speed = CalculateSpeedByMeters(CalculateDistance(tempPos));
        }

        private void DisableRenderer()
        {
            tutorialElements[currentIndex].OnConditionComplete -= GoNextIndex;
            StopAllCoroutines();
            EnableDisableRenderer(false);
            enabled = false;
        }

        private float CalculateDistance(Vector3 tempPos)
        {
            return Vector3.Distance(tempPos, destination.position);
        }

        private float CalculateSpeedByMeters(float distance)
        {
            return distance * speedMultiplier * Time.deltaTime;
        }

        private void EnableDisableRenderer(bool isEnable)
        {
            lineRenderer.enabled = isEnable;
        }
    }
}