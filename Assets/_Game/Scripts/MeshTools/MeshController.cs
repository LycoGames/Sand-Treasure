using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    [Range(1, 5f)] [SerializeField] private float radius;
    [Range(1, 5f)] [SerializeField] private float deformationStrength;

    private Mesh mesh;
    private Vector3[] verticies, modifiedVerts;

    private void Start()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        verticies = mesh.vertices;
        modifiedVerts = mesh.vertices;
        print(modifiedVerts.Length);
    }

    private void RecalculateMesh()
    {
        mesh.vertices = modifiedVerts;
        GetComponentInChildren<MeshCollider>().sharedMesh = mesh;
        mesh.RecalculateNormals();
    }

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            print(hit);
            for (int i = 0; i < modifiedVerts.Length; i++)
            {
                Vector3 distance = modifiedVerts[i] - hit.point;
                float smooth = 2f;
                float force = deformationStrength / (1f + hit.point.sqrMagnitude);

                if (distance.sqrMagnitude < radius)
                {
                    if (Input.GetMouseButton(0))
                    {
                        modifiedVerts[i] = modifiedVerts[i] + (Vector3.up * force) / smooth;
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        modifiedVerts[i] = modifiedVerts[i] + (Vector3.down * force) / smooth;
                    }
                }
            }
        }
        RecalculateMesh();
    }
}