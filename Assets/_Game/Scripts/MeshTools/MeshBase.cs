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
        public float X;
        public float Y;
        public float Z;

        public ModifiedVertex(float dataX, float dataY, float dataZ)
        {
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
        private Vector3[] vertices, modifiedVerts, baseVerts;
        private Dictionary<int, ModifiedVertex> modifiedVertexData = new();

        private void Start()
        {
            GetMeshVertices();
            foreach (var digZone in digZones) digZone.RegisterMeshBase(this);
        }

        public Vector3[] GetBaseVertices() => baseVerts;
        public Vector3[] GetModifiedVertices() => modifiedVerts;

        public void SetVertex(int index, Vector3 pos)
        {
            modifiedVerts[index] = pos;
            if (modifiedVertexData.ContainsKey(index))
                modifiedVertexData[index] = new ModifiedVertex(pos.x, pos.y, pos.z);
            else
                modifiedVertexData.Add(index, new ModifiedVertex(pos.x, pos.y, pos.z));
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
            baseVerts ??= modifiedVerts;
        }

        public object CaptureState()
        {
            return modifiedVertexData;
        }

        public void RestoreState(object state)
        {
            var list = (Dictionary<int, ModifiedVertex>)state;
            modifiedVertexData = list;
            mesh = meshFilter.mesh;
            var currentVertices = mesh.vertices;
            baseVerts = mesh.vertices;
            foreach (var data in list)
            {
                currentVertices[data.Key] = new Vector3(data.Value.X, data.Value.Y, data.Value.Z);
            }

            mesh.vertices = currentVertices;
            meshCollider.sharedMesh = mesh;
            mesh.RecalculateNormals();
        }
    }
}