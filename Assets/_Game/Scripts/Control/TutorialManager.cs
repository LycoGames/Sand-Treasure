using System;
using System.Collections.Generic;
using _Game.Scripts.BaseSequence;
using _Game.Scripts.Player;
using _Game.Scripts.Saving;
using _Game.Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.Control
{
    [Serializable]
    public struct TutorialManagerData
    {
        public int elementIndex;
        public bool isTutorialComplete;

        public TutorialManagerData(int elementIndex, bool isTutorialComplete)
        {
            this.elementIndex = elementIndex;
            this.isTutorialComplete = isTutorialComplete;
        }
    }

    public class TutorialManager : MonoBehaviour, ISaveable
    {
        [SerializeField] private CameraSwitcher camSwitcher;
        [SerializeField] private GameObject indicator;
        [SerializeField] private List<TutorialElement> tutorialElements;

        public bool IsTutorialComplete { get; private set; }

        private TutorialElement activeElement;
        private int elementIndex;

        private void OnEnable()
        {
            Actions.OnInGameStateBegin += StartTutorial;
        }

        private void OnDisable()
        {
            Actions.OnInGameStateBegin -= StartTutorial;
        }


        private void StartTutorial()
        {
            if (IsTutorialComplete) return;
            indicator.SetActive(true);
            ActivateTutorialElement();
        }

        private void ActivateTutorialElement()
        {
            if (activeElement != null)
            {
                activeElement.OnConditionComplete -= GoNextTutorialElement;
                activeElement.OnDestinationReached -= DeactivateIndicator;
            }

            if (elementIndex >= tutorialElements.Count)
            {
                IsTutorialComplete = true;
                DeactivateIndicator();
                return;
            }

            activeElement = tutorialElements[elementIndex];
            var dest = activeElement.GetDestinationTransform().position;
            dest.y = 8f;
            indicator.transform.position = dest;
            ActivateIndicator();
            activeElement.OnConditionComplete += GoNextTutorialElement;
            activeElement.OnDestinationReached += DeactivateIndicator;
            camSwitcher.ActivateCam(elementIndex);
        }

        private void GoNextTutorialElement()
        {
            elementIndex++;
            ActivateTutorialElement();
        }

        private void ActivateIndicator() => indicator.SetActive(true);
        private void DeactivateIndicator() => indicator.SetActive(false);

        public object CaptureState()
        {
            return new TutorialManagerData(elementIndex, IsTutorialComplete);
        }

        public void RestoreState(object state)
        {
            var data = (TutorialManagerData)state;
            elementIndex = data.elementIndex;
            IsTutorialComplete = data.isTutorialComplete;
        }
    }
}