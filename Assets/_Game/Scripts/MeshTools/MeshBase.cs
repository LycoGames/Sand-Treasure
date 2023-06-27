using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Saving;
using UnityEngine;

namespace _Game.Scripts.MeshTools
{
    [Serializable]
    public struct ModifiedVertex
    {
        public int Index;
        public float X;
        public float Y;
        public float Z;

        public ModifiedVertex(int index, float dataX, float dataY, float dataZ)
        {
            Index = index;
            X = dataX;
            Y = dataY;
            Z = dataZ;
        }
    }

    public class MeshBase : MonoBehaviour, ISaveable
    {
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private List<DigZone> digZones;

        private Mesh mesh;
        private Vector3[] vertices, modifiedVerts;
        private readonly List<ModifiedVertex> modifiedVertexData = new();

        private void Start()
        {
            GetMeshVertices();
            foreach (var digZone in digZones) digZone.RegisterMeshBase(this);
        }

        public Vector3[] GetVertices() => modifiedVerts;

        public void SetVertex(int index, Vector3 pos)
        {
            modifiedVerts[index] = pos;
            modifiedVertexData.Add(new ModifiedVertex(index, pos.x, pos.y, pos.z));
        }

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

        public object CaptureState()
        {
            return modifiedVertexData;
        }

        public void RestoreState(object state)
        {

            var list = (List<ModifiedVertex>)state;
            mesh = meshFilter.mesh;
            var currentVertices = mesh.vertices;
            foreach (var data in list)
            {
                currentVertices[data.Index] = new Vector3(data.X, data.Y, data.Z);
            }

            mesh.vertices = currentVertices;
            meshCollider.sharedMesh = mesh;
            mesh.RecalculateNormals();
        }
    }
}