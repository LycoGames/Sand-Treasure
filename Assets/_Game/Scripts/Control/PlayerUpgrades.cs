using System;
using System.Globalization;
using _Game.Scripts.Enums;
using _Game.Scripts.Player;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Control
{
    public class PlayerUpgrades : MonoBehaviour
    {
        [SerializeField] private PlayerUpgradesUI playerUpgradesUI;

        private Action OnUpgradeAction;
        private Stats playerStats;
        private Inventory playerInventory;

        //private Canvas canvas;


        public void Initialize(PlayerUpgradesUI playerUpgradesUI)
        {
            this.playerUpgradesUI = playerUpgradesUI;
            //canvas = this.playerUpgradesUI.GetComponent<Canvas>();
            DisableCanvas();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            playerStats = other.GetComponent<Stats>();
            playerInventory = other.GetComponent<Inventory>();
            Actions.onCollisionSellZone?.Invoke();
            SubscribeToOnUpgradeAction();
            SubscribeToButtonActions();
            SetUpgradesOnUI();
            EnableCanvas();
        }


        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            Actions.onCollisionSellZone?.Invoke();
            UnSubscribeToOnUpgradeAction();
            UnSubscribeToButtonActions();
            DisableCanvas();
        }

        private void SetUpgradesOnUI()
        {
            OnUpgradeAction?.Invoke();

            SetStackCapacityOnUI();
            SetDigFieldOnUI();
            SetItemDropChanceOnUI();
            SetStrengthOnUI();
        }


        private void UpgradeSelectedStat(Stat stat)
        {
            if (!IsPurchasable(stat))
                return;
            BuyUpgrade(stat);
            UpgradeStat(stat);
            switch (stat)
            {
                case Stat.StackCapacity:
                    SetStackCapacityOnUI();
                    break;
                case Stat.ItemDropChance:
                    SetItemDropChanceOnUI();
                    break;
                case Stat.DigField:
                    SetDigFieldOnUI();
                    break;
                case Stat.Strength:
                    SetStrengthOnUI();
                    break;
            }

            OnUpgradeAction?.Invoke();
        }

        private void SetStackCapacityOnUI()
        {
            var cost = playerStats.GetStatCost(Stat.StackCapacity).ToString();
            var level = playerStats.GetStatLevel(Stat.StackCapacity);
            print(level);
            if (IsStatOnMaxLevel(Stat.StackCapacity))
            {
                playerUpgradesUI.SetStackCapacity(level);
                return;
            }

            playerUpgradesUI.SetStackCapacity(cost, level);
        }

        private void SetDigFieldOnUI()
        {
            var cost = playerStats.GetStatCost(Stat.DigField).ToString();
            var level = playerStats.GetStatLevel(Stat.DigField);
            if (IsStatOnMaxLevel(Stat.DigField))
            {
                playerUpgradesUI.SetDigField(level);
                return;
            }

            playerUpgradesUI.SetDigField(cost, level);
        }

        private void SetStrengthOnUI()
        {
            var cost = playerStats.GetStatCost(Stat.Strength).ToString();
            var level = playerStats.GetStatLevel(Stat.Strength);
            if (IsStatOnMaxLevel(Stat.Strength))
            {
                playerUpgradesUI.SetStrength(level);
                return;
            }

            playerUpgradesUI.SetStrength(cost, level);
        }

        private void SetItemDropChanceOnUI()
        {
            var cost = playerStats.GetStatCost(Stat.ItemDropChance).ToString();
            var level = playerStats.GetStatLevel(Stat.ItemDropChance);
            if (IsStatOnMaxLevel(Stat.ItemDropChance))
            {
                playerUpgradesUI.SetItemDropChance(level);
                return;
            }

            playerUpgradesUI.SetItemDropChance(cost, level);
        }

        // ---------------- Upgrade With Texts-------------

        #region UpdateUpgradeUIwithTexts

        // private void SetStackCapacityOnUI()
        // {
        //     var cost = playerStats.GetStatCost(Stat.StackCapacity).ToString();
        //     var currentLevelStat = playerStats.GetStat(Stat.StackCapacity).ToString(CultureInfo.CurrentCulture);
        //     if (IsStatOnMaxLevel(Stat.StackCapacity))
        //     {
        //         playerUpgradesUI.SetStackCapacity(currentLevelStat);
        //         return;
        //     }
        //
        //     var nextLevelStat =
        //         playerStats.GetNextLevelStat(Stat.StackCapacity).ToString(CultureInfo.CurrentCulture);
        //     playerUpgradesUI.SetStackCapacity(cost, currentLevelStat, nextLevelStat);
        // }
        //
        // private void SetDigFieldOnUI()
        // {
        //     var cost = playerStats.GetStatCost(Stat.DigField).ToString();
        //     var level = playerStats.GetStatLevel(Stat.DigField);
        //     if (IsStatOnMaxLevel(Stat.DigField))
        //     {
        //         playerUpgradesUI.SetDigField(level);
        //         return;
        //     }
        //
        //     playerUpgradesUI.SetDigField(cost, level);
        // }
        //
        // private void SetStrengthOnUI()
        // {
        //     var cost = playerStats.GetStatCost(Stat.Strength).ToString();
        //     var level = playerStats.GetStatLevel(Stat.Strength);
        //     if (IsStatOnMaxLevel(Stat.Strength))
        //     {
        //         playerUpgradesUI.SetStrength(level);
        //         return;
        //     }
        //
        //     playerUpgradesUI.SetStrength(cost, level);
        // }
        //
        // private void SetItemDropChanceOnUI()
        // {
        //     var cost = playerStats.GetStatCost(Stat.ItemDropChance).ToString();
        //     var currentLevelStat = playerStats.GetStat(Stat.ItemDropChance).ToString(CultureInfo.CurrentCulture);
        //     if (IsStatOnMaxLevel(Stat.ItemDropChance))
        //     {
        //         playerUpgradesUI.SetItemDropChance(currentLevelStat);
        //         return;
        //     }
        //
        //     var nextLevelStat =
        //         playerStats.GetNextLevelStat(Stat.ItemDropChance).ToString(CultureInfo.CurrentCulture);
        //     playerUpgradesUI.SetItemDropChance(cost, currentLevelStat, nextLevelStat);
        // }

        #endregion

        private bool IsPurchasable(Stat stat)
        {
            var ownedMoney = playerInventory.GetMoney();
            return playerStats.GetStatLevel(stat) != playerStats.GetStatMaxLevel(stat) && ownedMoney >= GetCost(stat);
        }

        private bool IsStatOnMaxLevel(Stat stat)
        {
            return playerStats.GetStatLevel(stat) == playerStats.GetStatMaxLevel(stat);
        }

        private void BuyUpgrade(Stat stat)
        {
            int cost = playerStats.GetStatCost(stat);
            playerInventory.SpendMoney(cost);
        }

        private int GetCost(Stat stat)
        {
            return playerStats.GetStatCost(stat);
        }

        private void UpgradeStat(Stat stat)
        {
            playerStats.UpgradeStat(stat);
        }

        private void SetStackCapacityUpgradeButtonInteractable()
        {
            playerUpgradesUI.SetStackCapacityUpgradeButtonInteractable(IsPurchasable(Stat.StackCapacity));
        }

        private void SetDigFieldButtonInteractable()
        {
            playerUpgradesUI.SetDigFieldUpgradeButtonInteractable(IsPurchasable(Stat.DigField));
        }

        private void SetItemDropChanceButtonInteractable()
        {
            playerUpgradesUI.SetItemDropChanceUpgradeButtonInteractable(IsPurchasable(Stat.ItemDropChance));
        }

        private void SetStrengthButtonInteractable()
        {
            playerUpgradesUI.SetStrengthUpgradeButtonInteractable(IsPurchasable(Stat.Strength));
        }

        private void SubscribeToButtonActions()
        {
            playerUpgradesUI.OnStackCapacityUpgradeRequest += UpgradeSelectedStat;
            playerUpgradesUI.OnDigZoneUpgradeRequest += UpgradeSelectedStat;
            playerUpgradesUI.OnItemDropChanceUpgradeRequest += UpgradeSelectedStat;
            playerUpgradesUI.OnStrengthUpgradeRequest += UpgradeSelectedStat;
        }

        private void UnSubscribeToButtonActions()
        {
            playerUpgradesUI.OnStackCapacityUpgradeRequest -= UpgradeSelectedStat;
            playerUpgradesUI.OnDigZoneUpgradeRequest -= UpgradeSelectedStat;
            playerUpgradesUI.OnItemDropChanceUpgradeRequest -= UpgradeSelectedStat;
            playerUpgradesUI.OnStrengthUpgradeRequest -= UpgradeSelectedStat;
        }

        private void SubscribeToOnUpgradeAction()
        {
            OnUpgradeAction += SetDigFieldButtonInteractable;
            OnUpgradeAction += SetStackCapacityUpgradeButtonInteractable;
            OnUpgradeAction += SetItemDropChanceButtonInteractable;
            OnUpgradeAction += SetStrengthButtonInteractable;
        }

        private void UnSubscribeToOnUpgradeAction()
        {
            OnUpgradeAction -= SetDigFieldButtonInteractable;
            OnUpgradeAction -= SetStackCapacityUpgradeButtonInteractable;
            OnUpgradeAction -= SetItemDropChanceButtonInteractable;
            OnUpgradeAction -= SetStrengthButtonInteractable;
        }

        private void EnableCanvas()
        {
            playerUpgradesUI.OnEnter();
        }

        private void DisableCanvas()
        {
            playerUpgradesUI.OnExit();
        }
    }
}