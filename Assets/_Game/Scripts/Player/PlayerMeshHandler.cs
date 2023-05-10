using System.Collections;
using _Game.Scripts.MeshTools;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerMeshHandler : MonoBehaviour
    {
        [SerializeField] private MeshDeformer meshDeformer;
        [SerializeField] private float forceOffset;
        [SerializeField] private float force;
        [SerializeField] private float digCooldown;
        [SerializeField] private float diggingField;
        [SerializeField] private Transform diggerPos;
        private Coroutine cooldownTimer;
        private float time = Mathf.Infinity;

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("DigArea"))
        //     {
        //         //  DiggingState();
        //     }
        // }

        // private void DiggingState()
        // {
        //     //diggin anim & effects 
        //     DigCoroutine(GetHittedVertPoint());
        // }

        private Vector3 GetHittedVertPoint()
        {
            RaycastHit hit;
            if (Physics.Raycast(diggerPos.position, -diggerPos.up, out hit, 5f))
            {
                Debug.DrawRay(diggerPos.position, -diggerPos.up * 5, Color.blue, 5f);
                if (meshDeformer)
                {
                    print("found mesh deformer");
                    Vector3 hitPoint = hit.point;
                    hitPoint += hit.normal * forceOffset;
                    return hitPoint;
                }
            }

            print("zeroya düştü");
            return Vector3.negativeInfinity;
        }

        // IEnumerator DigCoroutine(Vector3 point)
        // {
        //     for (int i = 0; i < 100; i++)
        //     {
        //         meshDeformer.AddDeformingForce(point, force,diggingField);
        //         yield return new WaitForSeconds(0.1f);
        //     }
        // }

        // private void OnTriggerStay(Collider other)
        // {
        //     if (other.CompareTag("DigArea"))
        //     {
        //         Dig();
        //         time += Time.deltaTime;
        //     }
        // }

        private IEnumerator CooldownTimer()
        {
            while (true)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }

        public void StartTimerCoroutine()
        {
            cooldownTimer = StartCoroutine(CooldownTimer());
        }

        public void StopTimerCoroutine()
        {
            StopCoroutine(cooldownTimer);
        }

        public void Dig()
        {
            if (time > digCooldown)
            {
                meshDeformer.AddDeformingForce(diggerPos.position, force, diggingField);
                time = 0;
            }
        }
        
    }
}