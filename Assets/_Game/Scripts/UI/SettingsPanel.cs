using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private float closedSize;
        [SerializeField] private float openSize;
        [SerializeField] private float openClosedDuration;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Button settingsButton;
    
        private Tween currentTween;

        private void Start()
        {
            settingsButton.onClick.AddListener(OpenPanel);
        }

        private void OpenPanel()
        {
            currentTween.Kill();
            settingsButton.onClick.RemoveListener(OpenPanel);
            currentTween = rectTransform.DOSizeDelta(new Vector2(140, openSize), openClosedDuration);
            settingsButton.onClick.AddListener(ClosePanel);
        }

        private void ClosePanel()
        {
            currentTween.Kill();
            settingsButton.onClick.RemoveListener(ClosePanel);
            currentTween = rectTransform.DOSizeDelta(new Vector2(140, closedSize), openClosedDuration);
            settingsButton.onClick.AddListener(OpenPanel);
        }
    }
}