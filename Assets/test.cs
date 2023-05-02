using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using _Game.Scripts.Mesh;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private MeshDeformer meshDeformer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 3f))
            {
                Debug.DrawRay(transform.position, -transform.up, Color.blue, 5f);
                if (meshDeformer)
                {
                    print("found mesh deformer");
                    Vector3 point = hit.point;
                    point += hit.normal * 0.1f;
                    meshDeformer.AddDeformingForce(point, 5f);
                }
            }
        }
    }
}