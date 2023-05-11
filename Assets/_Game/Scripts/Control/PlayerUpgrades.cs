using System;
using System.Globalization;
using _Game.Scripts.Enums;
using _Game.Scripts.StatSystem;
using _Game.Scripts.UI;
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

        private void Start()
        {
            InitializeUI();
            DisableCanvas();
        }

        private void InitializeUI()
        {
            canvas = playerUpgradesUI.GetComponent<Canvas>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            playerStats = other.GetComponent<Stats>();
            playerInventory = other.GetComponent<Inventory>();
            SubscribeToOnUpgradeAction();
            SubscribeToButtonActions();
            SetUpgrades();
            EnableCanvas();
        }


        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            UnSubscribeToOnUpgradeAction();
            UnSubscribeToButtonActions();
            DisableCanvas();
        }


        // private void UpgradeHealth()
        // {
        //     if (!IsPurchasable(Stat.Health))
        //         return;
        //     BuyUpgrade(Stat.Health);
        //     UpgradeStat(Stat.Health);
        //     SetHealth();
        //     OnUpgradeAction?.Invoke();
        // }


        private void UpgradeMovementSpeed()
        {
            if (!IsPurchasable(Stat.MovementSpeed))
                return;
            BuyUpgrade(Stat.MovementSpeed);
            UpgradeStat(Stat.MovementSpeed);
            SetMovementSpeed();
            OnUpgradeAction?.Invoke();
        }

        private void UpgradeStackCapacity()
        {
            if (!IsPurchasable(Stat.StackCapacity))
                return;
            BuyUpgrade(Stat.StackCapacity);
            UpgradeStat(Stat.StackCapacity);
            SetStackCapacity();
            OnUpgradeAction?.Invoke();
        }

        private void SetMoney()
        {
        }

        private void SetStackCapacity()
        {
            var cost = playerStats.GetStatCost(Stat.StackCapacity).ToString();
            var currentLevelStat = playerStats.GetStat(Stat.StackCapacity).ToString(CultureInfo.CurrentCulture);
            if (IsLastLevel(Stat.StackCapacity))
            {
                playerUpgradesUI.SetStackCapacity(currentLevelStat);
                return;
            }

            var nextLevelStat =
                playerStats.GetNextLevelStat(Stat.StackCapacity).ToString(CultureInfo.CurrentCulture);
            playerUpgradesUI.SetStackCapacity(cost, currentLevelStat, nextLevelStat);
        }

        private void SetMovementSpeed()
        {
            var cost = playerStats.GetStatCost(Stat.MovementSpeed).ToString();
            var level = playerStats.GetStatLevel(Stat.MovementSpeed);
            playerUpgradesUI.SetMovementSpeed(cost, level);
        }

        // private void SetHealth()
        // {
        //     var cost = playerStats.GetStatCost(Stat.Health).ToString();
        //     var currentLevelStat = playerStats.GetStat(Stat.Health).ToString(CultureInfo.CurrentCulture);
        //     if (IsLastLevel(Stat.Health))
        //     {
        //         playerUpgradesUI.SetHealth(currentLevelStat);
        //         return;
        //     }
        //
        //     var nextLevelStat = playerStats.GetNextLevelStat(Stat.Health).ToString(CultureInfo.CurrentCulture);
        //     playerUpgradesUI.SetHealth(cost, currentLevelStat, nextLevelStat);
        // }


        private bool IsPurchasable(Stat stat)
        {
            var ownedMoney = playerInventory.GetMoney();
            return playerStats.GetStatLevel(stat) != playerStats.GetStatMaxLevel(stat) && ownedMoney >= GetCost(stat);
        }

        private bool IsLastLevel(Stat stat)
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

        // private void SetHealthUpgradeButtonInteractable()
        // {
        //     playerUpgradesUI.SetHealthUpgradeButtonInteractable(IsPurchasable(Stat.Health));
        // }

        private void SetUpgrades()
        {
            OnUpgradeAction?.Invoke();

            SetStackCapacity();
            SetMovementSpeed();
            //SetHealth();
        }

        private void SubscribeToButtonActions()
        {
            playerUpgradesUI.OnStackCapacityUpgradeRequest += UpgradeStackCapacity;
            playerUpgradesUI.OnMovementSpeedUpgradeRequest += UpgradeMovementSpeed;
            // playerUpgradesUI.OnHealthUpgradeRequest += UpgradeHealth;
        }

        private void UnSubscribeToButtonActions()
        {
            playerUpgradesUI.OnStackCapacityUpgradeRequest -= UpgradeStackCapacity;
            playerUpgradesUI.OnMovementSpeedUpgradeRequest -= UpgradeMovementSpeed;
            //playerUpgradesUI.OnHealthUpgradeRequest -= UpgradeHealth;
        }

        private void SubscribeToOnUpgradeAction()
        {
            OnUpgradeAction += SetMoney;
            //OnUpgradeAction += SetHealthUpgradeButtonInteractable;
            OnUpgradeAction += SetMovementSpeedButtonInteractable;
            OnUpgradeAction += SetStackCapacityUpgradeButtonInteractable;
        }

        private void UnSubscribeToOnUpgradeAction()
        {
            OnUpgradeAction -= SetMoney;
            // OnUpgradeAction -= SetHealthUpgradeButtonInteractable;
            OnUpgradeAction -= SetMovementSpeedButtonInteractable;
            OnUpgradeAction -= SetStackCapacityUpgradeButtonInteractable;
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