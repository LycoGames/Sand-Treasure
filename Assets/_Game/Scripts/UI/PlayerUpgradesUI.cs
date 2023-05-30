using System;
using _Game.Scripts.Enums;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class PlayerUpgradesUI : MonoBehaviour
    {
        [SerializeField] private RectTransform upgradesUIRectTransform;
        [SerializeField] private Canvas upgradesCanvas;

        public Action<Stat> OnStackCapacityUpgradeRequest;
        public Action<Stat> OnItemDropChanceUpgradeRequest;
        public Action<Stat> OnDigZoneUpgradeRequest;
        public Action<Stat> OnStrengthUpgradeRequest;

        [SerializeField] private TMP_Text stackCapacityCostText;
        [SerializeField] private Button stackCapacityUpgradeButton;
        [SerializeField] private TMP_Text stackCapacityStat;
        [SerializeField] private TMP_Text stackCapacityMaxTextField;
        [SerializeField] private GameObject stackCapacityUpgradableCostField;

        [Space] [SerializeField] private TMP_Text digFieldCostText;
        [SerializeField] private Button digFieldUpgradeButton;
        [SerializeField] private TMP_Text digFieldLevelText;
        [SerializeField] private TMP_Text digFieldMaxTextField;
        [SerializeField] private GameObject digFieldUpgradableCostField;


        [Space] [SerializeField] private TMP_Text itemDropChanceCostText;
        [SerializeField] private Button itemDropChanceUpgradeButton;
        [SerializeField] private TMP_Text itemDropChanceStat;
        [SerializeField] private TMP_Text itemDropChanceMaxTextField;
        [SerializeField] private GameObject itemDropChanceUpgradableCostField;

        [Space] [SerializeField] private TMP_Text strengthCostText;
        [SerializeField] private Button strengthUpgradeButton;
        [SerializeField] private TMP_Text strengthLevelText;
        [SerializeField] private TMP_Text strengthMaxTextField;
        [SerializeField] private GameObject strengthUpgradableCostField;

        private const string MaxText = "MAX";
        private const string LevelText = "LEVEL ";
        private const string StatSeparator = " -> ";
        private const string HealthText = "HP";
        private const string PcsText = "pcs";
        private const string Seconds = "sec";

        public void OnEnter()
        {
            upgradesCanvas.enabled = true;
            upgradesUIRectTransform.DOAnchorPos(Vector2.zero, 0.25f).SetAutoKill(true);
        }

        public void OnExit()
        {
            upgradesUIRectTransform.DOAnchorPos(new Vector2(1080, 0), 0.25f).SetAutoKill(true)
                .OnComplete((() => upgradesCanvas.enabled = false));
        }

        #region Changes

        private void Start()
        {
            stackCapacityUpgradeButton.onClick.AddListener(RequestStackCapacityUpgrade);
            digFieldUpgradeButton.onClick.AddListener(RequestDigFieldUpgrade);
            itemDropChanceUpgradeButton.onClick.AddListener(RequestItemDropChanceUpgrade);
            strengthUpgradeButton.onClick.AddListener(RequestStrengthUpgrade);
        }

        public void SetStackCapacity(string cost, string currentLevelStat, string nextLevelStat)
        {
            stackCapacityCostText.text = cost;
            stackCapacityStat.text = currentLevelStat + PcsText + StatSeparator + nextLevelStat + PcsText;
        }

        public void SetStackCapacity(string currentLevelStat)
        {
            stackCapacityUpgradableCostField.SetActive(false);
            stackCapacityMaxTextField.enabled = true;
            stackCapacityStat.text = currentLevelStat + PcsText;
        }

        public void SetItemDropChance(string cost, string currentLevelStat, string nextLevelStat)
        {
            itemDropChanceCostText.text = cost;
            itemDropChanceStat.text = currentLevelStat + Seconds + StatSeparator + nextLevelStat + Seconds;
        }

        public void SetItemDropChance(string currentLevelStat)
        {
            itemDropChanceUpgradableCostField.SetActive(false);
            itemDropChanceMaxTextField.enabled = true;
            itemDropChanceStat.text = currentLevelStat + Seconds;
        }

        public void SetDigField(string cost, int level)
        {
            digFieldCostText.text = cost;
            digFieldLevelText.text = LevelText + level;
        }

        public void SetDigField(int level)
        {
            digFieldUpgradableCostField.SetActive(false);
            digFieldMaxTextField.enabled = true;
            digFieldLevelText.text = LevelText + level;
        }

        public void SetStrength(string cost, int level)
        {
            strengthCostText.text = cost;
            strengthLevelText.text = LevelText + level;
        }

        public void SetStrength(int level)
        {
            strengthUpgradableCostField.SetActive(false);
            strengthMaxTextField.enabled = true;
            strengthLevelText.text = LevelText + level;
        }

        public void SetStackCapacityUpgradeButtonInteractable(bool isInteractable)
        {
            if (isInteractable != stackCapacityUpgradeButton.interactable)
                stackCapacityUpgradeButton.interactable = isInteractable;
        }

        public void SetItemDropChanceUpgradeButtonInteractable(bool isInteractable)
        {
            if (isInteractable != itemDropChanceUpgradeButton.interactable)
                itemDropChanceUpgradeButton.interactable = isInteractable;
        }

        public void SetDigFieldUpgradeButtonInteractable(bool isInteractable)
        {
            if (isInteractable != digFieldUpgradeButton.interactable)
                digFieldUpgradeButton.interactable = isInteractable;
        }
        public void SetStrengthUpgradeButtonInteractable(bool isInteractable)
        {
            if (isInteractable != strengthUpgradeButton.interactable)
                strengthUpgradeButton.interactable = isInteractable;
        }
        #endregion

        #region Requests

        private void RequestStackCapacityUpgrade()
        {
            OnStackCapacityUpgradeRequest?.Invoke(Stat.StackCapacity);
        }

        private void RequestItemDropChanceUpgrade()
        {
            OnItemDropChanceUpgradeRequest?.Invoke(Stat.ItemDropChance);
        }

        private void RequestDigFieldUpgrade()
        {
            OnDigZoneUpgradeRequest?.Invoke(Stat.DigField);
        }

        private void RequestStrengthUpgrade()
        {
            OnStrengthUpgradeRequest?.Invoke(Stat.Strength);
        }

        #endregion
    }
}