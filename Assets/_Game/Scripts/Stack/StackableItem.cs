using _Game.Scripts.Control.Items;
using _Game.Scripts.Enums;
using _Game.Scripts.Pool;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.Stack
{
    public class StackableItem : Item
    {
        [SerializeField] private ItemType type;
        [SerializeField] private int value = 1;
       // [SerializeField] private Collider myCollider;
        [SerializeField] private Vector3 offset;


        public ItemType Type => type;
        public Vector3 Offset => offset;

        public int Value
        {
            get => value;
            set => this.value = value;
        }

        public Coroutine MoveCoroutine { get; set; }

        private ObjectPool pool;

        public Sequence DropSequence;

        public override void Setup(ObjectPool pool)
        {
            this.pool = pool;
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void PickUp(StackData stackData)
        {
            if (MoveCoroutine != null)
                StopCoroutine(MoveCoroutine);
            if (DropSequence is { active: true })
                DropSequence.Kill();
            SetRotation(stackData.transform.rotation);
            transform.SetParent(stackData.gameObject.transform);
            // if (myCollider && myCollider.isTrigger)
            // {
            //    myCollider.isTrigger = false;
            // }
        }

        public void ReSendToPool()
        {
            pool.Add(this);
        }

        public void ActivateColliderTrigger()
        {
           // myCollider.isTrigger = true;
        }
    }
}