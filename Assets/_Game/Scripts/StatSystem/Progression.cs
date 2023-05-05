using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Enums;
using UnityEngine;

namespace _Game.Scripts.StatSystem
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionData[] progressionData = null;

        private Dictionary<Stat, float[]> lookupTable = null;
        private Dictionary<Stat, int[]> costLookupTable = null;

        public float GetStat(Stat stat, int level)
        {
            BuildLookup();
            float[] levels = lookupTable[stat];
            if (!levels.Any())
                return 0;
            return levels.Length < level ? levels.Last() : levels[level - 1];
        }

        public int GetStatCost(Stat stat, int level)
        {
            BuildCostLookup();
            int[] levels = costLookupTable[stat];
            if (!levels.Any())
                return 0;
            return levels.Length < level ? levels.Last() : levels[level - 1];
        }

        public int GetLevels(Stat stat)
        {
            BuildLookup();
            float[] levels = lookupTable[stat];
            return levels.Length;
        }

        public IEnumerable<Stat> GetStatFields()
        {
            BuildLookup();
            return lookupTable.Keys;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<Stat, float[]>();
            foreach (ProgressionData data in progressionData)
            {
                lookupTable[data.stat] = data.levels;
            }
        }

        private void BuildCostLookup()
        {
            if (costLookupTable != null) return;
            costLookupTable = new Dictionary<Stat, int[]>();
            foreach (ProgressionData data in progressionData)
            {
                costLookupTable[data.stat] = data.upgradeCosts;
            }
        }
    }

    [Serializable]
    class ProgressionData
    {
        public Stat stat;
        public float[] levels;
        public int[] upgradeCosts;
    }
}