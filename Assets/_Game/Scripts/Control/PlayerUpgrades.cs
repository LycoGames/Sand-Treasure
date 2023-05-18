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

        private Canvas canvas;


        public void Initialize(PlayerUpgradesUI playerUpgradesUI)
        {
            this.playerUpgradesUI = playerUpgradesUI;
            canvas = this.playerUpgradesUI.GetComponent<Canvas>();
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
            SetMovementSpeedOnUI();
            SetItemDropChanceOnUI();
            //SetHealth();
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
                case Stat.MovementSpeed:
                    SetMovementSpeedOnUI();
                    break;
            }

            OnUpgradeAction?.Invoke();
        }

        private void SetStackCapacityOnUI()
        {
            var cost = playerStats.GetStatCost(Stat.StackCapacity).ToString();
            var currentLevelStat = playerStats.GetStat(Stat.StackCapacity).ToString(CultureInfo.CurrentCulture);
            if (IsStatOnMaxLevel(Stat.StackCapacity))
            {
                playerUpgradesUI.SetStackCapacity(currentLevelStat);
                return;
            }

            var nextLevelStat =
                playerStats.GetNextLevelStat(Stat.StackCapacity).ToString(CultureInfo.CurrentCulture);
            playerUpgradesUI.SetStackCapacity(cost, currentLevelStat, nextLevelStat);
        }

        private void SetMovementSpeedOnUI()
        {
            var cost = playerStats.GetStatCost(Stat.MovementSpeed).ToString();
            var level = playerStats.GetStatLevel(Stat.MovementSpeed);
            if (IsStatOnMaxLevel(Stat.MovementSpeed))
            {
                playerUpgradesUI.SetMovementSpeed(level);
                return;
            }

            playerUpgradesUI.SetMovementSpeed(cost, level);
        }

        private void SetItemDropChanceOnUI()
        {
            var cost = playerStats.GetStatCost(Stat.ItemDropChance).ToString();
            var currentLevelStat = playerStats.GetStat(Stat.ItemDropChance).ToString(CultureInfo.CurrentCulture);
            if (IsStatOnMaxLevel(Stat.ItemDropChance))
            {
                playerUpgradesUI.SetItemDropChance(currentLevelStat);
                return;
            }

            var nextLevelStat =
                playerStats.GetNextLevelStat(Stat.ItemDropChance).ToString(CultureInfo.CurrentCulture);
            playerUpgradesUI.SetItemDropChance(cost, currentLevelStat, nextLevelStat);
        }

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

        private void SetMovementSpeedButtonInteractable()
        {
            playerUpgradesUI.SetMovementSpeedUpgradeButtonInteractable(IsPurchasable(Stat.MovementSpeed));
        }

        private void SetItemDropChanceButtonInteractable()
        {
            playerUpgradesUI.SetItemDropChanceUpgradeButtonInteractable(IsPurchasable(Stat.ItemDropChance));
        }

        private void SubscribeToButtonActions()
        {
            playerUpgradesUI.OnStackCapacityUpgradeRequest += UpgradeSelectedStat;
            playerUpgradesUI.OnMovementSpeedUpgradeRequest += UpgradeSelectedStat;
            playerUpgradesUI.OnItemDropChanceUpgradeRequest += UpgradeSelectedStat;
        }

        private void UnSubscribeToButtonActions()
        {
            playerUpgradesUI.OnStackCapacityUpgradeRequest -= UpgradeSelectedStat;
            playerUpgradesUI.OnMovementSpeedUpgradeRequest -= UpgradeSelectedStat;
            playerUpgradesUI.OnItemDropChanceUpgradeRequest -= UpgradeSelectedStat;
        }

        private void SubscribeToOnUpgradeAction()
        {
            OnUpgradeAction += SetMovementSpeedButtonInteractable;
            OnUpgradeAction += SetStackCapacityUpgradeButtonInteractable;
            OnUpgradeAction += SetItemDropChanceButtonInteractable;
        }

        private void UnSubscribeToOnUpgradeAction()
        {
            OnUpgradeAction -= SetMovementSpeedButtonInteractable;
            OnUpgradeAction -= SetStackCapacityUpgradeButtonInteractable;
            OnUpgradeAction -= SetItemDropChanceButtonInteractable;
        }

        private void EnableCanvas()
        {
            canvas.enabled = true;
        }

        private void DisableCanvas()
        {
            canvas.enabled = false;
        }
    }
}