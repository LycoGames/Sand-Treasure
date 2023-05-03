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
        private float time = Mathf.Infinity;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DigArea"))
            {
                print(other.name);
                //  DiggingState();
            }
        }

        private void DiggingState()
        {
            //diggin anim & effects 
            DigCoroutine(GetHittedVertPoint());
        }

        private Vector3 GetHittedVertPoint()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 3f))
            {
                Debug.DrawRay(transform.position, -transform.up, Color.blue, 5f);
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

        IEnumerator DigCoroutine(Vector3 point)
        {
            for (int i = 0; i < 100; i++)
            {
                meshDeformer.AddDeformingForce(point, force);
                yield return new WaitForSeconds(0.1f);
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
                meshDeformer.AddDeformingForce(GetHittedVertPoint(), force);
                time = 0;
            }
        }
    }
}