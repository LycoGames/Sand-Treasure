using UnityEngine;

namespace _Game.Scripts.Stack
{
    public class LootableStackableItem : StackableItem
    {
        [SerializeField] private int dropChance;
        public int DropChance => dropChance;
    }
}