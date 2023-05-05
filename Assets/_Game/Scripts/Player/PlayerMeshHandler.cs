using System;
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
        private float time = Mathf.Infinity;
        private WaitForSeconds wfsForDig;

        private void Start()
        {
            wfsForDig = new WaitForSeconds(digCooldown);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DigArea"))
            {
                print(other.name);
                StartCoroutine(DigCoroutine());
                //  DiggingState();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("DigArea"))
            {
                StopAllCoroutines();
            }
        }

        private void DiggingState()
        {
            //diggin anim & effects 
            //DigCoroutine(GetHittedVertPoint());
        }

        private Vector3 GetHittedVertPoint()
        {
            RaycastHit hit;
            if (Physics.Raycast(diggerPos.position, -diggerPos.up, out hit, 5f))
            {
                Debug.DrawRay(diggerPos.position, -diggerPos.up*5, Color.blue, 5f);
                if (meshDeformer)
                {
                    print("found mesh deformer");
                    Vector3 hitPoint = hit.point;
                    hitPoint += hit.normal * forceOffset;
                    return hitPoint;
                }
            }

            return Vector3.zero;
        }

        // IEnumerator DigCoroutine(Vector3 point)
        // {
        //     for (int i = 0; i < 100; i++)
        //     {
        //         meshDeformer.AddDeformingForce(point, force,diggingField);
        //         yield return new WaitForSeconds(0.1f);
        //     }
        // }
        IEnumerator DigCoroutine()
        {
            while (true)
            {
                Debug.Log("Digging");
                meshDeformer.AddDeformingForce(diggerPos.position, force,diggingField);
                yield return wfsForDig;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("DigArea"))
            {
                Dig();
                time += Time.deltaTime;
            }
        }


        private void Dig()
        {
            if (time > digCooldown)
            {
                meshDeformer.AddDeformingForce(diggerPos.position, force,diggingField);
                time = 0;
            }
        }
    }
}