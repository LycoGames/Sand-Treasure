using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Utils
{
    public class UIRewardVisualizer : MonoBehaviour
    {
        [SerializeField] private RectTransform destination;
        [SerializeField] private RectTransform imageToInstantiatePrefab;
        [SerializeField] private float xRandomOffset = 50;
        
        private const float BounceOffset = 500f;
        private Vector2 scaleDown = new(.6f, .6f);

        public Transform Parent { get; set; }

        private Camera mainCam;
        private float screenWidth;
        public void SetDestination(RectTransform rectTransform) => destination=rectTransform;
        private void Start()
        {
            mainCam = Camera.main;
            screenWidth = Screen.width;
        }

        public void VisualiseReward(Vector3 position, Action onSequenceCompleted = null)
        {
            var screenPos = mainCam.WorldToScreenPoint(position);
            var instance = Instantiate(imageToInstantiatePrefab, screenPos, Quaternion.identity,destination);
            var targetPos = screenPos;
            targetPos.y = Screen.height*3/5;
            targetPos.x = Math.Clamp(targetPos.x + GetRandomXOffset(), 30, screenWidth - 30);
            var rewardSequence = DOTween.Sequence();
            var flyUpSequence = DOTween.Sequence();
            var punchSequence = DOTween.Sequence();
            punchSequence.Append(instance.DOScale(scaleDown, .25f));
            punchSequence.Append(instance.DOScale(Vector3.one, .25f)).SetLoops(-1);
            flyUpSequence.Append(instance.DOMove(targetPos, 1f)).OnComplete(() => punchSequence.Kill(true));
            rewardSequence.Append(flyUpSequence);
            rewardSequence.Append(instance.DOMove(destination.position, .3f).SetEase(Ease.Linear));
            rewardSequence.OnComplete(() =>
            {
                onSequenceCompleted?.Invoke();
                Destroy(instance.gameObject);
            });
        }

        private float GetRandomXOffset()
        {
            return Random.Range(-xRandomOffset, xRandomOffset);
        }
    }
}