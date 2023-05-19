using System;
using _Game.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class PlayerUpgradesUI : MonoBehaviour
    {
        public Action<Stat> OnStackCapacityUpgradeRequest;
        public Action<Stat> OnItemDropChanceUpgradeRequest;
        public Action<Stat> OnMovementSpeedUpgradeRequest;

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


        private const string MaxText = "MAX";
        private const string LevelText = "LEVEL ";
        private const string StatSeparator = " -> ";
        private const string HealthText = "HP";
        private const string PcsText = "pcs";
        private const string Seconds = "sec";

        #region Changes

        private void Start()
        {
            stackCapacityUpgradeButton.onClick.AddListener(RequestStackCapacityUpgrade);
            digFieldUpgradeButton.onClick.AddListener(RequestDigFieldUpgrade);
            itemDropChanceUpgradeButton.onClick.AddListener(RequestItemDropChanceUpgrade);
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
            OnMovementSpeedUpgradeRequest?.Invoke(Stat.DigField);
        }

        #endregion
    }
}