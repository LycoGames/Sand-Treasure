
using _Game.Scripts.Enums;

namespace _Game.Scripts.Stack
{
    public interface IStackItemCollector
    {
        public ItemType GetItemType();
        public bool CanAddItem();
        public void Add(StackableItem stackableItem);
    }
}

