using System;
using System.Globalization;
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
        [SerializeField] private RectTransform finger;
        public RectTransform Finger => finger;

        [SerializeField] private TMP_Text stackCapacityCostText;
        [SerializeField] private Button stackCapacityUpgradeButton;

        [SerializeField] private Image stackCapacityLevelBar;

        // [SerializeField] private TMP_Text stackCapacityStat;
        [SerializeField] private TMP_Text stackCapacityMaxTextField;
        [SerializeField] private GameObject stackCapacityUpgradableCostField;
        [SerializeField] private TMP_Text stackLevelText;


        [Space] [SerializeField] private TMP_Text digFieldCostText;
        [SerializeField] private Button digFieldUpgradeButton;

        [SerializeField] private Image digFieldLevelBar;

        //  [SerializeField] private TMP_Text digFieldLevelText;
        [SerializeField] private TMP_Text digFieldMaxTextField;
        [SerializeField] private GameObject digFieldUpgradableCostField;
        [SerializeField] private TMP_Text digFieldLevelText;


        [Space] [SerializeField] private TMP_Text itemDropChanceCostText;
        [SerializeField] private Button itemDropChanceUpgradeButton;

        [SerializeField] private Image itemDropChanceLevelBar;

        //  [SerializeField] private TMP_Text itemDropChanceStat;
        [SerializeField] private TMP_Text itemDropChanceMaxTextField;
        [SerializeField] private GameObject itemDropChanceUpgradableCostField;

        [Space] [SerializeField] private TMP_Text strengthCostText;
        [SerializeField] private Button strengthUpgradeButton;

        [SerializeField] private Image strengthLevelBar;

        // [SerializeField] private TMP_Text strengthLevelText;
        [SerializeField] private TMP_Text strengthMaxTextField;
        [SerializeField] private GameObject strengthUpgradableCostField;
        [SerializeField] private TMP_Text strengthLevelText;

        private const string MaxText = "MAX";
        private const string LevelText = "LEVEL ";
        private const string LvlText = "Lvl.";
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

        // ----------------Upgrade with level bars-------------------

        public void SetStackCapacity(string cost, int level)
        {
            stackCapacityCostText.text = cost;
            float newLevel = level - 1;
            string lvl = newLevel.ToString(CultureInfo.InvariantCulture);
            stackLevelText.text = LvlText + lvl;
            newLevel %= 10; //reset bar each 10 level
            stackCapacityLevelBar.fillAmount = (newLevel / 10);
        }

        public void SetStackCapacity(int level)
        {
            stackCapacityUpgradableCostField.SetActive(false);
            stackCapacityMaxTextField.enabled = true;
            stackCapacityLevelBar.fillAmount = 1f;
            stackLevelText.text = MaxText;
        }

        public void SetItemDropChance(string cost, int level)
        {
            itemDropChanceCostText.text = cost;
            itemDropChanceLevelBar.fillAmount = (float)(level - 1) / 10;
        }

        public void SetItemDropChance(int level)
        {
            itemDropChanceUpgradableCostField.SetActive(false);
            itemDropChanceMaxTextField.enabled = true;
            itemDropChanceLevelBar.fillAmount = 1;
        }

        public void SetDigField(string cost, int level)
        {
            digFieldCostText.text = cost;
            float newLevel = level - 1;
            string lvl = newLevel.ToString(CultureInfo.InvariantCulture);
            digFieldLevelText.text = LvlText + lvl;
            newLevel %= 10;
            digFieldLevelBar.fillAmount = (newLevel / 10);
        }

        public void SetDigField(int level)
        {
            digFieldUpgradableCostField.SetActive(false);
            digFieldMaxTextField.enabled = true;
            digFieldLevelBar.fillAmount = 1f;
            digFieldLevelText.text = MaxText;
        }


        public void SetStrength(string cost, int level)
        {
            strengthCostText.text = cost;
            float newLevel = level - 1;
            string lvl = newLevel.ToString(CultureInfo.InvariantCulture);
            strengthLevelText.text = LvlText + lvl;
            newLevel %= 10;
            strengthLevelBar.fillAmount = (newLevel / 10);
        }

        public void SetStrength(int level)
        {
            strengthUpgradableCostField.SetActive(false);
            strengthMaxTextField.enabled = true;
            strengthLevelBar.fillAmount = 1f;
            strengthLevelText.text = MaxText;
        }


        // ----------------Upgrade with texts-------------------

        #region upgradeUIwithexts

        // public void SetStackCapacity(string cost, string currentLevelStat, string nextLevelStat)
        // {
        //     stackCapacityCostText.text = cost;
        //     stackCapacityStat.text = currentLevelStat + PcsText + StatSeparator + nextLevelStat + PcsText;
        // }
        //
        // public void SetStackCapacity(string currentLevelStat)
        // {
        //     stackCapacityUpgradableCostField.SetActive(false);
        //     stackCapacityMaxTextField.enabled = true;
        //     stackCapacityStat.text = currentLevelStat + PcsText;
        // }
        //
        // public void SetItemDropChance(string cost, string currentLevelStat, string nextLevelStat)
        // {
        //     itemDropChanceCostText.text = cost;
        //     itemDropChanceStat.text = currentLevelStat + Seconds + StatSeparator + nextLevelStat + Seconds;
        // }
        //
        // public void SetItemDropChance(string currentLevelStat)
        // {
        //     itemDropChanceUpgradableCostField.SetActive(false);
        //     itemDropChanceMaxTextField.enabled = true;
        //     itemDropChanceStat.text = currentLevelStat + Seconds;
        // }
        //
        // public void SetDigField(string cost, int level)
        // {
        //     digFieldCostText.text = cost;
        //     digFieldLevelText.text = LevelText + level;
        // }
        //
        // public void SetDigField(int level)
        // {
        //     digFieldUpgradableCostField.SetActive(false);
        //     digFieldMaxTextField.enabled = true;
        //     digFieldLevelText.text = LevelText + level;
        // }
        //
        // public void SetStrength(string cost, int level)
        // {
        //     strengthCostText.text = cost;
        //     strengthLevelText.text = LevelText + level;
        // }
        //
        // public void SetStrength(int level)
        // {
        //     strengthUpgradableCostField.SetActive(false);
        //     strengthMaxTextField.enabled = true;
        //     strengthLevelText.text = LevelText + level;
        // }

        #endregion


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