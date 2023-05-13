using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private List<IObserver> observers=new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    protected void NotifyObservers(int value,ItemType type)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(value,type);
        }
    }
}
