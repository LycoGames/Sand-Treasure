using UnityEngine;

namespace _Game.Scripts.MeshTools
{
    public class MeshDeformer : MonoBehaviour
    {
        private Mesh mesh;
        private Vector3[] vertices, modifiedVerts;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private MeshFilter meshFilter;

        private Vector3 currentvertPos;
        private float currentDistance;
        private float newHeight;

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
        }

        private void UpdateMesh()
        {
            mesh.vertices = modifiedVerts;
            meshCollider.sharedMesh = mesh;
            mesh.RecalculateNormals();
        }


        public void AddDeformingForce(Vector3 point, float force, float diggingField)
        {
            for (int i = 0; i < modifiedVerts.Length; i++)
            {
                AddForceToVertices(i, point, force, diggingField);
            }

            UpdateMesh();
        }

        private void AddForceToVertices(int i, Vector3 point, float force, float diggingField)
        {
            currentvertPos = GetScaledVector(modifiedVerts[i]);
            currentDistance = Vector3.Distance(point, currentvertPos);
            // with out height, only 2d circle field !!!
            //currentDistance = Vector3.Distance(new Vector3(point.x,0,point.z)
            //   , new Vector3(currentvertPos.x,0,currentvertPos.z));
            if (currentDistance > diggingField || modifiedVerts[i].y <= 0) return;
            newHeight = modifiedVerts[i].y - force * (diggingField - currentDistance) * Time.deltaTime;
            modifiedVerts[i].y = Mathf.Clamp(newHeight, 0, modifiedVerts[i].y);
        }

        private Vector3 GetScaledVector(Vector3 vector)
        {
            vector.x *= transform.localScale.x;
            vector.y *= transform.localScale.y;
            vector.z *= transform.localScale.z;
            return vector;
        }
    }
}