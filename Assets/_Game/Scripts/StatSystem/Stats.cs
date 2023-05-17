using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Datas;
using _Game.Scripts.Enums;
using _Game.Scripts.Saving;
using UnityEngine;

namespace _Game.Scripts.StatSystem
{
    public class Stats : MonoBehaviour, ISerializationCallbackReceiver, ISaveable
    {
        public Action<float> OnMovementSpeedChange;
        public Action<float> OnStackCapacityChange;
        public Action<float> OnTransferSpeedChange;
        public Action<float> OnStackLimitChange;
        public Action<float> OnItemDropChanceChange;
        
        [SerializeField] private Progression progression;

        [SerializeField] private string id;
        [SerializeField] private bool saveStatsForActiveMap;

        private Dictionary<Stat, int> statLevelsDictionary = new Dictionary<Stat, int>();

        public string ID => id;
        public bool SaveStatsForActiveMap => saveStatsForActiveMap;


        public float GetStat(Stat stat)
        {
            if (!HaveStat(stat))
                statLevelsDictionary[stat] = 1;
            return progression.GetStat(stat, statLevelsDictionary[stat]);
        }

        public float GetNextLevelStat(Stat stat)
        {
            if (!HaveStat(stat))
                statLevelsDictionary[stat] = 1;
            return progression.GetStat(stat, statLevelsDictionary[stat] + 1);
        }

        public int GetStatMaxLevel(Stat stat)
        {
            return progression.GetLevels(stat);
        }

        public int GetStatLevel(Stat stat)
        {
            if (!HaveStat(stat))
                statLevelsDictionary[stat] = 1;
            return statLevelsDictionary[stat];
        }

        public int GetStatCost(Stat stat)
        {
            if (!HaveStat(stat))
                statLevelsDictionary[stat] = 1;
            return progression.GetStatCost(stat, statLevelsDictionary[stat]);
        }

        public void UpgradeStat(Stat stat)
        {
            if (!HaveStat(stat))
                statLevelsDictionary[stat] = 2;
            else
                statLevelsDictionary[stat]++;


            InvokeUpdateAction(stat);
        }

        private bool HaveStat(Stat stat)
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
            return !HaveStat(stat) || statLevelsDictionary[stat] == progression.GetLevels(stat);
        }

        private void InvokeUpdateAction(Stat stat)
        {
            switch (stat)
            {
                case Stat.MovementSpeed:
                    OnMovementSpeedChange?.Invoke(GetStat(stat));
                    break;
                case Stat.StackCapacity:
                    OnStackCapacityChange?.Invoke(GetStat(stat));
                    break;
                case Stat.TransferSpeed:
                    OnTransferSpeedChange?.Invoke(GetStat(stat));
                    break;
                case Stat.ItemDropChance:
                    OnItemDropChanceChange?.Invoke(GetStat(stat));
                    break;
                default:
                    Debug.LogWarning("Stat have no action");
                    break;
            }
        }

        public object CaptureState()
        {
            return statLevelsDictionary;
        }

        public void RestoreState(object state)
        {
            statLevelsDictionary = (Dictionary<Stat, int>)state;
        }
    }
}