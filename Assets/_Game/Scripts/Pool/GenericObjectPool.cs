using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private int count;
    [SerializeField] private protected  T objectToPool;
    public Queue<T> pooledObjects = new Queue<T>();
    public List<T> objectsInUse = new List<T>();

    protected void Awake()
    {
        InitializePool(count);
    }

    protected virtual void InitializePool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var item = Instantiate(objectToPool, this.transform);
            item.gameObject.SetActive(false);
            pooledObjects.Enqueue(item);
        }
    }

    public virtual T GetFromPool()
    {
        if (pooledObjects.Count>0)
        {
            T itemToReturn = pooledObjects.Dequeue();
            objectsInUse.Add(itemToReturn);
            itemToReturn.gameObject.SetActive(true);
            return itemToReturn;
        }

        return CreateNewPooledItem();
    }

    protected virtual T CreateNewPooledItem()
    {
        T newPooledItem = Instantiate(objectToPool, this.transform);
        objectsInUse.Add(newPooledItem);
        return newPooledItem;
    }

    public virtual void ReturnToPool(T item)
    {
        item.gameObject.SetActive(false);
        pooledObjects.Enqueue(item);
        objectsInUse.Remove(item);
    }

    public virtual void ReturnAllItemsToPool()
    {
        foreach (var item in objectsInUse)
        {
            item.gameObject.SetActive(false);
            pooledObjects.Enqueue(item);
        }
        objectsInUse.Clear();
    }
}
