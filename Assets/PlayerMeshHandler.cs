using System.Collections;
using _Game.Scripts.Mesh;
using UnityEngine;

public class PlayerMeshHandler : MonoBehaviour
{
    [SerializeField] private MeshDeformer meshDeformer;
    [SerializeField] private float  forceOffset;
    [SerializeField] private float force;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DigArea"))
        {
            DiggingState();
        }
    }
    
    private void DiggingState()
    {
        //diggin anim & effects 
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 3f))
        {
            Debug.DrawRay(transform.position, -transform.up, Color.blue, 5f);
            if (meshDeformer)
            {
                print("found mesh deformer");
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                StartCoroutine(DigCoroutine(point, force));
            }
        }
    }

    IEnumerator DigCoroutine(Vector3 point, float force)
    {
        for (int i = 0; i < 20; i++)
        {
            meshDeformer.AddDeformingForce(point, force);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
