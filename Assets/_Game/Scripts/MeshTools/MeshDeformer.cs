using UnityEngine;

namespace _Game.Scripts.MeshTools
{
    public class MeshDeformer : MonoBehaviour
    {
        private Mesh mesh;
        private Vector3[] vertices, modifiedVerts;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private MeshFilter meshFilter;
        private void Start()
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


        private void Update()
        {
            mesh.vertices = modifiedVerts;
            meshCollider.sharedMesh = mesh;
            mesh.RecalculateNormals();
        }


        public void AddDeformingForce(Vector3 point, float force,float diggingField)
        {
            for (int i = 0; i < modifiedVerts.Length; i++)
            {
                AddForceToVertecies(i, point, force,diggingField);
            }
        }

        private void AddForceToVertecies(int i, Vector3 point, float force,float diggingField)
        {
            Vector3 vertPos = modifiedVerts[i];
            vertPos.x*=transform.localScale.x;
            vertPos.y *= transform.localScale.y;
            vertPos.z *= transform.localScale.z;
            if (Vector3.Distance(point,vertPos ) > diggingField) return;
            Vector3 pointToVertex = modifiedVerts[i] - point;
            float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
            modifiedVerts[i] += (Vector3.down * attenuatedForce) / 2f;
            if (modifiedVerts[i].y <= 0)
            {
                modifiedVerts[i].y = 0;
            }
        }
    }
}