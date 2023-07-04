using System.Collections.Generic;
using _Game.Scripts.BaseSequence;
using _Game.Scripts.Enums;
using _Game.Scripts.Saving;
using Unity.VisualScripting;
using UnityEngine;

namespace _Game.Scripts.MeshTools
{
    public class DigZone : MonoBehaviour, ISaveable
    {
        private MeshBase meshBase;
        private Dictionary<int, Vector3> zoneVertices = new Dictionary<int, Vector3>();
        private Dictionary<int, Vector3> modifiedZoneVertices;
        private float totalDigHeight;
        private float currentDugHeight;
        private Vector3 meshScale;
        private Vector3 scaledPos;


        private Vector3 currentvertPos;
        private float currentDistance;
        private float newHeight;
        private EndGameState endGameState;

        public void RegisterMeshBase(MeshBase meshBase)
        {
            this.meshBase = meshBase;
            SearchInZoneVertices();
            endGameState=SequenceManager.Instance.GetAppState(AppStateTypes.EndGame) as EndGameState;
            endGameState.OnClickNextLevelButton += ClearModifiedData;
        }

        private void ClearModifiedData()
        {
            currentDugHeight=0;
        }

        public void AddDeformingForce(Vector3 point, float force, float diggingField)
        {
            if (currentDugHeight >= totalDigHeight) return;
            modifiedZoneVertices = new Dictionary<int, Vector3>(zoneVertices);
            foreach (var vertex in zoneVertices)
            {
                AddForceToVertices(vertex.Key, vertex.Value, point, force, diggingField);
            }

            zoneVertices = new Dictionary<int, Vector3>(modifiedZoneVertices);
            modifiedZoneVertices.Clear();
            meshBase.UpdateMesh();
        }

        public float GetPercentOfDig()
        {
            return currentDugHeight / totalDigHeight * 100;
        }

        private void SearchInZoneVertices()
        {
            var baseVertices = meshBase.GetBaseVertices();
            var modifiedVertices = meshBase.GetModifiedVertices();
            meshScale = meshBase.transform.localScale;
            scaledPos = new Vector3();
            //Debug.Log(modifiedVertices.Length);
            // float zoneSize = transform.localScale.x / 2;
            for (int i = 0; i < modifiedVertices.Length; i++)
            {
                scaledPos = GetScaledVector(modifiedVertices[i], meshScale);
                // if (Vector3.Distance(scaledPos, transform.position) <= zoneSize)
                // {
                //     zoneVertices.Add(i, vertices[i]);
                //     totalDigHeight += vertices[i].y;
                // }
                zoneVertices.Add(i, modifiedVertices[i]);
                totalDigHeight += baseVertices[i].y;
            }
        }

        private void AddForceToVertices(int index, Vector3 vertex, Vector3 point, float force, float diggingField)
        {
            currentvertPos = GetScaledVector(vertex, meshScale);
            currentDistance = Vector3.Distance(point, currentvertPos);
            if (currentDistance > diggingField || vertex.y <= 0) return;
            newHeight = vertex.y - force * (diggingField - currentDistance) * 0.01f;
            currentDugHeight += vertex.y - Mathf.Clamp(newHeight, 0, vertex.y);
            vertex.y = Mathf.Clamp(newHeight, 0, vertex.y);
            modifiedZoneVertices[index] = vertex;
            meshBase.SetVertex(index, vertex);
        }

        private Vector3 GetScaledVector(Vector3 vector, Vector3 scale)
        {
            vector.x *= scale.x;
            vector.y *= scale.y;
            vector.z *= scale.z;
            return vector;
        }

        public object CaptureState()
        {
            return currentDugHeight;
        }

        public void RestoreState(object state)
        {
            currentDugHeight = (float)state;
        }
    }
}