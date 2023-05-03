using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Tools
{
    public class TerrainToMeshConverter : MonoBehaviour
    {
        [SerializeField] private Terrain terrain;
        [SerializeField] private GameObject convertToObject;

        void Start()
        {
            var bounds = terrain.terrainData.bounds;
            var meshFilter = convertToObject.GetComponent<MeshFilter>();
            var mesh = meshFilter.mesh;
            List<Vector3> convertedObjectVertices = new List<Vector3>();
            foreach (var vert in mesh.vertices)
            {
                var pos = convertToObject.transform.localToWorldMatrix * vert;
                var newVert = vert;
                newVert.y = terrain.SampleHeight(pos);
                convertedObjectVertices.Add(newVert);
            }

            mesh.SetVertices(convertedObjectVertices.ToArray());
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
        }
    }
}