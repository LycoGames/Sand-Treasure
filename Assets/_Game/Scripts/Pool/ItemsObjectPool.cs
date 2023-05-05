using _Game.Scripts.Stack;

namespace _Game.Scripts.Pool
{
    public class ItemsObjectPool : GenericObjectPool<StackableItem>
    {
        public StackableItem GetPrefab()
        {
            return base.objectToPool;
        }
    }
}