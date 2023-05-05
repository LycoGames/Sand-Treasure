using System.Collections;
using _Game.Scripts.Enums;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using _Game.Scripts.StatSystem;
using UnityEngine;

namespace _Game.Scripts.Control
{
    public class Factory : MonoBehaviour, IStackItemGiver
    {
        [SerializeField] protected StackManager stackManager;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] protected StackableItem prefab;
        [SerializeField] protected float transferDelay;
        [SerializeField] protected float focusTime = 0.5f;

        private ObjectPool pool;
        protected Stats stats;
        private Vector3 startPosition;
        private Quaternion startRotation;
        private WaitForSeconds produceDelayWaitForSeconds;
        private Coroutine produceCoroutine;


        private void Awake()
        {
            SetStats();
        }

        private void OnEnable()
        {
            stats.OnTransferSpeedChange += UpdateTransferDelay;
        }


        private void OnDisable()
        {
            stats.OnTransferSpeedChange -= UpdateTransferDelay;
        }

        protected void Start()
        {
            SetItemStartPositionAndRotation();
            SetPool(prefab);
            StartCoroutine(ProduceItemCoroutine());
            UpdateTransferDelay(transferDelay);
        }


        public ItemType GetItemType()
        {
            return prefab.Type;
        }

        public bool CanGetItem()
        {
            return stackManager.CanGetFromStack(GetItemType());
        }

        public virtual StackableItem Get()
        {
            return stackManager.Get(GetItemType());
        }

        public StackableItem GetItemFromPool()
        {
            var stackableItem = pool.Get();
            if (stackableItem == null) return null;
            stackableItem.SetPosition(startPosition);
            stackableItem.SetRotation(startRotation);

            stackableItem.gameObject.SetActive(true);
            return stackableItem;
        }

        protected IEnumerator ProduceItemCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(focusTime);
                while (stackManager.CanAddToStack(prefab.Type))
                {
                    StackableItem item = GetItemFromPool();
                    item.SetPosition(spawnPoint.position);
                    item.SetRotation(spawnPoint.rotation);
                    stackManager.Add(item, transferDelay);
                    yield return produceDelayWaitForSeconds;
                }
            }
        }

        private void UpdateTransferDelay(float delay)
        {
            produceCoroutine ??= StartCoroutine(ProduceItemCoroutine());
            transferDelay = delay;
            SetProduceDelayWaitForSeconds();
        }

        protected void SetItemStartPositionAndRotation()
        {
            startPosition = spawnPoint.position;
            startRotation = spawnPoint.rotation;
        }

        protected void SetProduceDelayWaitForSeconds()
        {
            produceDelayWaitForSeconds = new WaitForSeconds(transferDelay);
        }

        protected void SetPool(StackableItem product)
        {
            foreach (ObjectPool poolInstance in FindObjectsOfType<ObjectPool>())
            {
                var stackableItem = poolInstance.GetPrefab();

                if (stackableItem != null && stackableItem.Type == product.Type)
                    pool = poolInstance;
            }

            if (pool == null)
                Debug.LogError(name + " There is no pool for item that you want to use");
        }

        private void SetStats()
        {
            stats = GetComponent<Stats>();
        }
    }
}