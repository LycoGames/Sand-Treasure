using _Game.Scripts.Base.UserInterface;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class EndGameUI : AbstractBaseCanvas
    {
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private RectTransform nextLevelButtonRectTransform;
    
        public OnUIButtonClickEvent OnClickNextLevel;

        public override void Start()
        {
            nextLevelButton.onClick.AddListener(GoNextLevel);
        }

        public override void OnStart()
        {
            Debug.Log("EndGameUI Enter");
            nextLevelButtonRectTransform.DOAnchorPos(new Vector2(-170,0),0.25f);
        }

        public override void OnQuit()
        {
            Debug.Log("EndGameUI Exit");
            nextLevelButtonRectTransform.DOAnchorPos(new Vector2(170,0),0.25f);
        }

        private void GoNextLevel()
        {
            OnClickNextLevel?.Invoke();
        }
    }
}