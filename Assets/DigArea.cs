using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Control.Items;
using _Game.Scripts.Enums;
using _Game.Scripts.Pool;
using _Game.Scripts.Stack;
using UnityEngine;

public class DigArea : MonoBehaviour
{
    [SerializeField] private float diggingCooldown = .25f;
    [SerializeField] private LootArea lootArea;

    private StateController stateController;
    private Coroutine diggingCoroutine;
    private WaitForSeconds diggingCoroutineWaitForSeconds;
    private StackManager playerStackManager;
    private List<ItemsObjectPool> pools;
    private Dictionary<ItemType, ItemsObjectPool> poolDictionary;

    private void Start()
    {
        diggingCoroutineWaitForSeconds = new WaitForSeconds(diggingCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stateController == null || playerStackManager == null || pools == null || poolDictionary == null)
        {
            stateController = other.GetComponent<StateController>();
            playerStackManager = other.GetComponent<StackManager>();
            pools = other.GetComponent<PoolCreator>().ItemsObjectPools;
            SetPoolsDictionary();
        }


        diggingCoroutine = StartCoroutine(DiggingCoroutine());
    }

    private void SetPoolsDictionary()
    {
        poolDictionary = new Dictionary<ItemType, ItemsObjectPool>();
        foreach (var pool in pools)
        {
            poolDictionary[pool.PoolType] = pool;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (diggingCoroutine != null)
        {
            StopCoroutine(diggingCoroutine);
        }
    }

    private IEnumerator DiggingCoroutine()
    {
        while (true)
        {
            if (stateController.CurrentState == stateController.DigState)
            {
                var item = lootArea.GetDroppedItem();
                if (item == null)
                {
                    continue;
                }

                if (playerStackManager.CanAddToStack(item.Type))
                {
                    var obj = poolDictionary[item.Type].GetFromPool();
                    playerStackManager.Add(obj, diggingCooldown);
                }
            }

            yield return diggingCoroutineWaitForSeconds;
        }
    }
}