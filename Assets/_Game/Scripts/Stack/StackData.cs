using System.Collections.Generic;
using _Game.Scripts.Enums;
using UnityEngine;

namespace _Game.Scripts.Stack
{
    public class StackData : MonoBehaviour
    {
        public Stack<StackableItem> Stack { get; } = new Stack<StackableItem>();

        public ItemType StackType { get; set; }

        public Vector3 ItemBounds { get; private set; }

        public void SetItemBounds(Renderer itemRenderer)
        {
            var itemRendererBounds = itemRenderer.bounds;
            ItemBounds = new Vector3(itemRendererBounds.size.x, itemRendererBounds.size.y, itemRendererBounds.size.z);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}
