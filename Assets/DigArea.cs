using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        diggingCoroutineWaitForSeconds = new WaitForSeconds(diggingCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stateController == null || playerStackManager == null || pools == null)
        {
            stateController = other.GetComponent<StateController>();
            playerStackManager = other.GetComponent<StackManager>();
            pools = other.GetComponent<PoolCreator>().ItemsObjectPools;
        }

        stateController.ChangeState(stateController.DigState);
        diggingCoroutine = StartCoroutine(DiggingCoroutine());
    }

    private void OnTriggerExit(Collider other)
    {
        stateController.ChangeState(stateController.IdleState);
        if (diggingCoroutine != null)
        {
            StopCoroutine(diggingCoroutine);
        }
    }

    private IEnumerator DiggingCoroutine()
    {
        while (true)
        {
            var item = lootArea.GetDroppedItem();
            if (item==null)
            {
                continue;
            }
            foreach (var pool in pools)
            {
                if (pool.PoolType == item.Type && playerStackManager.CanAddToStack(item.Type))
                {
                    var obj = pool.GetFromPool();
                    obj.gameObject.SetActive(true);
                    playerStackManager.Add(obj, diggingCooldown);
                }
            }
            
            // var obj = pools[0].GetFromPool();
            // obj.gameObject.SetActive(true);
            // playerStackManager.Add(obj, diggingCooldown);
            yield return diggingCoroutineWaitForSeconds;
        }
    }
}