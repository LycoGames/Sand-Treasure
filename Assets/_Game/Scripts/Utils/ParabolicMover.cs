using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Utils
{
    public class ParabolicMover : MonoBehaviour
    {
        public static ParabolicMover Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Coroutine Move(Transform item, Vector3 startPosition, float height,
            float duration, Vector3 offset)
        {
            return StartCoroutine(Coroutine(item, startPosition, height, duration, offset));
        }

        public IEnumerator Coroutine(Transform item, Vector3 startPosition, float height,
            float duration, Vector3 offset)
        {
            float step = 0;
            while (step < duration)
            {
                step += Time.deltaTime;
                item.localPosition =
                    ParabolicMovement.Parabola(startPosition, offset, height,
                        step / duration);
                yield return null;
            }

            item.localPosition = offset;
        }
    }
}