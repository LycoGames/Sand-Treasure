using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class PlayerUpgradesUI : MonoBehaviour
    {
        public Action OnStackCapacityUpgradeRequest;
        public Action OnHealthUpgradeRequest;
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


        [Space] [SerializeField] private TMP_Text healthCostText;
        [SerializeField] private Button healthUpgradeButton;
        [SerializeField] private TMP_Text healthStat;
        [SerializeField] private TMP_Text healthMaxTextField;
        [SerializeField] private GameObject healthUpgradableCostField;


        private const string MaxText = "MAX";
        private const string LevelText = "LEVEL ";
        private const string StatSeparator = "->";
        private const string HealthText = "HP";
        private const string PcsText = "pcs";

        #region Changes

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

        public void SetHealth(string cost, string currentLevelStat, string nextLevelStat)
        {
            healthCostText.text = cost;
            healthStat.text = currentLevelStat + HealthText + StatSeparator + nextLevelStat + HealthText;
        }

        public void SetHealth(string currentLevelStat)
        {
            healthUpgradableCostField.SetActive(false);
            healthMaxTextField.enabled = true;
            healthStat.text = currentLevelStat + HealthText;
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

        public void SetHealthUpgradeButtonInteractable(bool isInteractable)
        {
            if (isInteractable != healthUpgradeButton.interactable)
                healthUpgradeButton.interactable = isInteractable;
        }

        public void SetMovementSpeedUpgradeButtonInteractable(bool isInteractable)
        {
            if (isInteractable != movementSpeedUpgradeButton.interactable)
                movementSpeedUpgradeButton.interactable = isInteractable;
        }

        #endregion

        #region Requests

        public void RequestStackCapacityUpgrade()
        {
            OnStackCapacityUpgradeRequest?.Invoke();
        }

        public void RequestHealthUpgrade()
        {
            OnHealthUpgradeRequest?.Invoke();
        }

        public void RequestMovementSpeedUpgrade()
        {
            OnMovementSpeedUpgradeRequest?.Invoke();
        }

        #endregion
    }
}