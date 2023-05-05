using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Datas;
using _Game.Scripts.Enums;
using UnityEngine;

namespace _Game.Scripts.StatSystem
{
    public class Stats : MonoBehaviour, ISerializationCallbackReceiver
    {
        public Action<float> OnMovementSpeedChange;
        public Action<float> OnHealthChange;
        public Action<float> OnStackCapacityChange;
        public Action<float> OnTransferSpeedChange;
        public Action<float> OnStackLimitChange;
        public Action<float> OnDamageChange;

        [SerializeField] private Progression progression;

        [SerializeField] private string id;
        [SerializeField] private bool saveStatsForActiveMap;

        private Dictionary<Stat, int> statLevelsDictionary = new Dictionary<Stat, int>();

        public string ID => id;
        public bool SaveStatsForActiveMap => saveStatsForActiveMap;


        public void Setup(Dictionary<Stat, int> dictionary)
        {
            statLevelsDictionary = dictionary;
            InvokeAllUpdateActions();
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, statLevelsDictionary[stat]);
        }

        public float GetNextLevelStat(Stat stat)
        {
            return progression.GetStat(stat, statLevelsDictionary[stat] + 1);
        }

        public int GetStatMaxLevel(Stat stat)
        {
            return progression.GetLevels(stat);
        }

        public int GetStatLevel(Stat stat)
        {
            return statLevelsDictionary[stat];
        }

        public int GetStatCost(Stat stat)
        {
            return progression.GetStatCost(stat, statLevelsDictionary[stat]);
        }

        public void UpgradeStat(Stat stat)
        {
            statLevelsDictionary[stat]++;
            InvokeUpdateAction(stat);
        }

        public bool HaveStat(Stat stat)
        {
            return statLevelsDictionary.ContainsKey(stat);
        }

        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
        }

        public StatData CreateDefaultData()
        {
            var statDataConfigList = progression.GetStatFields().Select(stat => new StatDataConfig(stat, 1)).ToList();
            var statData = new StatData(id, statDataConfigList);
            return statData;
        }

        public bool IsStatOnMaxLevel(Stat stat)
        {
            return statLevelsDictionary[stat] == progression.GetLevels(stat);
        }

        private void InvokeUpdateAction(Stat stat)
        {
            switch (stat)
            {
                case Stat.MovementSpeed:
                    OnMovementSpeedChange?.Invoke(GetStat(stat));
                    break;
                case Stat.Health:
                    OnHealthChange?.Invoke(GetStat(stat));
                    break;
                case Stat.StackCapacity:
                    OnStackCapacityChange?.Invoke(GetStat(stat));
                    break;
                case Stat.TransferSpeed:
                    OnTransferSpeedChange?.Invoke(GetStat(stat));
                    break;
                case Stat.StackLimit:
                    OnStackLimitChange?.Invoke(GetStat(stat));
                    break;
                case Stat.Damage:
                    OnDamageChange?.Invoke(GetStat(stat));
                    break;
                default:
                    Debug.LogWarning("Stat have no action");
                    break;
            }
        }

        private void InvokeAllUpdateActions()
        {
            foreach (var stat in progression.GetStatFields())
            {
                InvokeUpdateAction(stat);
            }
        }
    }
}