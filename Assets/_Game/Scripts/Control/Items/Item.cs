using _Game.Scripts.Pool;
using UnityEngine;

namespace _Game.Scripts.Control.Items
{
    public abstract class Item : MonoBehaviour
    {
        public abstract void Setup(ObjectPool pool);
    }
}