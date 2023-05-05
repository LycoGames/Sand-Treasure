
using _Game.Scripts.Enums;

namespace _Game.Scripts.Stack
{
    public interface IStackItemGiver
    {
        public ItemType GetItemType();
        public bool CanGetItem();
        public StackableItem Get();
    }
}

