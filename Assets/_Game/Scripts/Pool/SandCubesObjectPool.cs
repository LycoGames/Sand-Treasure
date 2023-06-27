using _Game.Scripts.Enums;
using _Game.Scripts.UI;
using UnityEngine;

namespace _Game.Scripts.Pool
{
    public class SandCubesObjectPool : GenericObjectPool<SandCubes>
    {
        public SandType sandType;
        private InGameUI inGameUI;

        public void SetSandType(SandCubes sandCube, InGameUI inGameUI)
        {
            base.objectToPool = sandCube;
            sandType = sandCube.SandType;
            this.inGameUI = inGameUI;
        }

        protected override void InitializePool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var item = Instantiate(objectToPool, this.transform);
                item.gameObject.SetActive(false);
                pooledObjects.Enqueue(item);
                var rw = item.RewardVisualizer;
                rw.SetDestination(inGameUI.MoneyPanel);
            }
        }

        public override SandCubes GetFromPool()
        {
            if (pooledObjects.Count > 0)
            {
                var itemToReturn = pooledObjects.Dequeue();
                objectsInUse.Add(itemToReturn);
                //itemToReturn.gameObject.SetActive(true);
                return itemToReturn;
            }

            return CreateNewPooledItem();
        }

        protected override SandCubes CreateNewPooledItem()
        {
            SandCubes newPooledItem = Instantiate(objectToPool, this.transform);
            objectsInUse.Add(newPooledItem);
            newPooledItem.RewardVisualizer.SetDestination(inGameUI.MoneyPanel);
            return newPooledItem;
        }

        public override void ReturnToPool(SandCubes sandCube)
        {
            base.ReturnToPool(sandCube);
            sandCube.transform.parent = this.transform;
        }
    }
}