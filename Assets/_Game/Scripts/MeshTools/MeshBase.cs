using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.MeshTools
{
    public class MeshBase : MonoBehaviour
    {
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private List<DigZone> digZones;

        private Mesh mesh;
        private Vector3[] vertices, modifiedVerts;

        private void Start()
        {
            GetMeshVertices();
            foreach (var digZone in digZones) digZone.RegisterMeshBase(this);
        }

        public Vector3[] GetVertices() => modifiedVerts;

        public void SetVertex(int index, Vector3 pos) => modifiedVerts[index] = pos;

        public void UpdateMesh()
        {
            mesh.vertices = modifiedVerts;
            meshCollider.sharedMesh = mesh;
            mesh.RecalculateNormals();
        }

        private void GetMeshVertices()
        {
            mesh = meshFilter.mesh;
            vertices = mesh.vertices;
            modifiedVerts = mesh.vertices;
            modifiedVerts = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                modifiedVerts[i] = vertices[i];
            }
        }
    }
}