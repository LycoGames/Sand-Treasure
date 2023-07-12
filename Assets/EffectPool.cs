using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Player;
using _Game.Scripts.Pool;
using UnityEngine;

public class EffectPool : GenericObjectPool<Effects>
{
    public static EffectPool Instance;
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    public override void ReturnToPool(Effects item)
    {
        base.ReturnToPool(item);
        item.gameObject.SetActive(false);
        pooledObjects.Enqueue(item);
        objectsInUse.Remove(item);
    }
}
