using _Game.Scripts.Enums;
using _Game.Scripts.Stack;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerCollector : MonoBehaviour
    {
        [SerializeField] StackManager stackManager;
        [SerializeField] private float transferSpeed;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Diamond") && stackManager.CanAddToStack(ItemType.Money))
            {
                StackableItem stackableItem = other.GetComponent<StackableItem>();
                stackManager.Add(stackableItem, transferSpeed);
            }
        }
    }
}