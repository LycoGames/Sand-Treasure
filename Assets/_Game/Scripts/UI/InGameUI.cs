using System;
using _Game.Scripts.Base.UserInterface;
using _Game.Scripts.Enums;
using _Game.Scripts.Observer;
using _Game.Scripts.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class InGameUI : AbstractBaseCanvas, IObserver
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Inventory inventory;
        [SerializeField] private RectTransform moneyPanel;

        public RectTransform MoneyPanel => moneyPanel;
        [SerializeField] private RectTransform treasurePanel;
        public RectTransform TreasurePanel => treasurePanel;
        [SerializeField] private TextMeshProUGUI treasureTotalText;
      
        [SerializeField] private TextMeshProUGUI foundTreasureText;

        [SerializeField] private Toggle audioToggle;
        public OnUIToggleClickEvent AudioToggle;
        private readonly Vector3 punchScale = new Vector3(0.3f, 0.3f, 0.3f);

        private readonly Vector3 stockScale = Vector3.one;

        private void Start()
        {
            inventory.AddObserver(this);
            moneyText.text = inventory.GetMoney().ToString();
            audioToggle.onValueChanged.AddListener(ToggleAudioOnOff);
        }
        
        public void SetTotalTreasureCount(int count)
        {
            treasureTotalText.text = count.ToString();
        }

        public void SetFoundedTreasureCount(int count)
        {
            foundTreasureText.text = count.ToString();
        }
        public override void OnStart()
        {
            Debug.Log("InGameUI OnStart");
        }

        public override void OnQuit()
        {
            Debug.Log("InGameUI OnExit");
        }

        public void OnNotify(int value, ItemType type)
        {
            switch (type)
            {
                case ItemType.Money:
                    DOTween.Sequence()
                        .Append(moneyPanel.transform.DOPunchScale(punchScale, 0.1f, 2)).SetEase(Ease.InFlash)
                        .Append(moneyPanel.transform.DOScale(stockScale, 0.1f));
                    moneyText.text = value.ToString();
                    break;
                case ItemType.Treasure:
                    DOTween.Sequence()
                        .Append(treasurePanel.transform.DOPunchScale(punchScale, 0.1f, 2)).SetEase(Ease.InFlash)
                        .Append(treasurePanel.transform.DOScale(stockScale, 0.1f));
                    SetFoundedTreasureCount(value);
                    break;
            }
        }

        private void ToggleAudioOnOff(bool isOn)
        {
            AudioToggle?.Invoke(isOn);
        }
    }
}