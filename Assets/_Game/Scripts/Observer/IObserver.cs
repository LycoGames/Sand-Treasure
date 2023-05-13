using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Enums;
using UnityEngine;

public interface IObserver
{
    public void OnNotify(int value,ItemType type);
}
