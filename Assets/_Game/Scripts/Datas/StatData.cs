using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Enums;

namespace _Game.Scripts.Datas
{
    [Serializable]
    public class StatData
    {
        public string id;
        public List<StatDataConfig> statDataList;

        public StatData(string id, List<StatDataConfig> configList)
        {
            this.id = id;
            statDataList = configList;
        }

        public void UpgradeStat(Stat stat)
        {
            foreach (var statData in statDataList.Where(statData => statData.stat == stat))
            {
                statData.level++;
                break;
            }
        }
    }

    [Serializable]
    public class StatDataConfig
    {
        public Stat stat;
        public int level;

        public StatDataConfig(Stat stat, int level)
        {
            this.stat = stat;
            this.level = level;
        }
    }
}