using System;
using _Game.Scripts.Enums;
using _Game.Scripts.Observer;
using _Game.Scripts.Saving;

namespace _Game.Scripts.Player
{
    public class Inventory : Subject, ISaveable
    {
        private int money;
        private int treasure;

        private void Start()
        {
            AddMoney(30000);
        }

        public void AddMoney(int value)
        {
            money += value;
            base.NotifyObservers(money, ItemType.Money);
        }

        public void SpendMoney(int value)
        {
            money -= value;
            base.NotifyObservers(money, ItemType.Money);
        }

        public void AddTreasure()
        {
            treasure++;
            GameManager.Instance.IncreaseFoundedTreasureCount();
            base.NotifyObservers(treasure, ItemType.Treasure);
        }

        public int GetMoney()
        {
            return money;
        }

        public bool HasEnoughMoneyToSpend(int value)
        {
            return money >= value;
        }

        public object CaptureState()
        {
            return money;
        }

        public void RestoreState(object state)
        {
            money = (int)state;
            base.NotifyObservers(money, ItemType.Money);
        }

        public void ResetTreasureCount()
        {
            treasure = 0;
        }
    }
}