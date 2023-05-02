using UnityEngine;

namespace _Game.Scripts.Mesh
{
    public class MeshDeformer : MonoBehaviour
    {
        private UnityEngine.Mesh mesh;
        private Vector3[] verticies, modifiedVerts;

        private void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            verticies = mesh.vertices;
            modifiedVerts = mesh.vertices;
            modifiedVerts = new Vector3[verticies.Length];
            for (int i = 0; i < verticies.Length; i++)
            {
                modifiedVerts[i] = verticies[i];
            }
        }


        private void Update()
        {
            mesh.vertices = modifiedVerts;
            mesh.RecalculateNormals();
        }


        public void AddDeformingForce(Vector3 point, float force)
        {
            for (int i = 0; i < modifiedVerts.Length; i++)
            {
                AddForceToVertecies(i, point, force);
            }
        }

        private void AddForceToVertecies(int i, Vector3 point, float force)
        {
            Vector3 pointToVertex = modifiedVerts[i] - point;
            float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
            modifiedVerts[i] = modifiedVerts[i] + (Vector3.down * attenuatedForce) / 2f;
            if (modifiedVerts[i].y <= 0)
            {
                modifiedVerts[i].y = 0;
            }
        }
    }
}