using System;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using JetBrains.Annotations;

namespace _Game.Scripts.Datas
{
    [Serializable]
    public class StatsData
    {
        public List<StatData> statsDataList;

        [CanBeNull]
        public StatData FindStatDataByID(string id)
        {
            return statsDataList.Exists(statData => statData.id == id)
                ? statsDataList.Find(statData => statData.id == id)
                : null;
        }

        public void UpgradeStat(string id, Stat stat)
        {
            FindStatDataByID(id)?.UpgradeStat(stat);
        }

        public void ClearData(string id)
        {
            var statData = FindStatDataByID(id);
            if (statData != null)
                statsDataList.Remove(statData);
        }
    }
}