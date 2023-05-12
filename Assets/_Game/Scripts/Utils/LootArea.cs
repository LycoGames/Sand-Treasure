using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Stack;
using UnityEngine;

public class LootArea : MonoBehaviour
{
    [SerializeField] private List<LootableStackableItem> itemsThatCanBeDrop;

    public StackableItem GetDroppedItem()
    {
        List<StackableItem> possibleItems = new List<StackableItem>();
        int randomNumber = Random.Range(0, 101);
        foreach (var item in itemsThatCanBeDrop)
        {
            if (randomNumber<=item.DropChance)
            {
                possibleItems.Add(item);
            }
        }

        if (possibleItems.Count>0)
        {
            StackableItem droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        print("No item dropped");
        return null;
    }
}