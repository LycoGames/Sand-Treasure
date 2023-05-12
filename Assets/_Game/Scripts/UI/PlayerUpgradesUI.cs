using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class PlayerUpgradesUI : MonoBehaviour
    {
        public Action OnStackCapacityUpgradeRequest;
        public Action OnItemDropChanceUpgradeRequest;
        public Action OnMovementSpeedUpgradeRequest;

        [SerializeField] private TMP_Text stackCapacityCostText;
        [SerializeField] private Button stackCapacityUpgradeButton;
        [SerializeField] private TMP_Text stackCapacityStat;
        [SerializeField] private TMP_Text stackCapacityMaxTextField;
        [SerializeField] private GameObject stackCapacityUpgradableCostField;

        [Space] [SerializeField] private TMP_Text movementSpeedCostText;
        [SerializeField] private Button movementSpeedUpgradeButton;
        [SerializeField] private TMP_Text movementSpeedLevelText;
        [SerializeField] private TMP_Text movementSpeedMaxTextField;
        [SerializeField] private GameObject movementSpeedUpgradableCostField;


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
            movementSpeedUpgradeButton.onClick.AddListener(RequestMovementSpeedUpgrade);
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

        public void SetMovementSpeed(string cost, int level)
        {
            movementSpeedCostText.text = cost;
            movementSpeedLevelText.text = LevelText + level;
        }

        public void SetMovementSpeed(int level)
        {
            movementSpeedUpgradableCostField.SetActive(false);
            movementSpeedMaxTextField.enabled = true;
            movementSpeedLevelText.text = LevelText + level;
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

        public void SetMovementSpeedUpgradeButtonInteractable(bool isInteractable)
        {
            if (isInteractable != movementSpeedUpgradeButton.interactable)
                movementSpeedUpgradeButton.interactable = isInteractable;
        }

        #endregion

        #region Requests

        private void RequestStackCapacityUpgrade()
        {
            OnStackCapacityUpgradeRequest?.Invoke();
        }

        private void RequestItemDropChanceUpgrade()
        {
            OnItemDropChanceUpgradeRequest?.Invoke();
        }

        private void RequestMovementSpeedUpgrade()
        {
            OnMovementSpeedUpgradeRequest?.Invoke();
        }

        #endregion
    }
}