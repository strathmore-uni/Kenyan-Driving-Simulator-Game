using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peque.Traffic
{
    public class RoadGenerator : MonoBehaviour
    {
        public int roadLength = 100; // length of the road in meters
        public int roadWidth = 10; // width of the road in meters
        public int waypointInterval = 10; // interval between waypoints in meters
        public GameObject waypointPrefab; // prefab for the waypoints

        private List<Vector3> roadPoints = new List<Vector3>(); // list of road points
        public List<GameObject> waypoints = new List<GameObject>(); // list of waypoints

        private void Start()
        {
            GenerateRoad();
        }

        private void GenerateRoad()
        {
            // Generate the road points
            for (int i = 0; i < roadLength; i++)
            {
                float x = i * roadWidth / roadLength;
                float z = 0;
                roadPoints.Add(new Vector3(x, 0, z));
            }

            // Create the road mesh
            Mesh roadMesh = new Mesh();
            roadMesh.SetVertices(roadPoints.ToArray()); // Convert List to array
            roadMesh.SetTriangles(GetTriangles(roadPoints), 0);
            GameObject roadObject = new GameObject("Road");
            roadObject.AddComponent<MeshFilter>().mesh = roadMesh;
            roadObject.AddComponent<MeshRenderer>();

            // Create waypoints at regular intervals
            for (int i = 0; i < roadLength; i += waypointInterval)
            {
                Vector3 waypointPosition = roadPoints[i];
                GameObject waypoint = Instantiate(waypointPrefab, waypointPosition, Quaternion.identity);
                waypoints.Add(waypoint);
            }
        }

        private int[] GetTriangles(List<Vector3> points)
        {
            // implementation for generating triangles
            int[] triangles = new int[(points.Count - 1) * 6];
            for (int i = 0; i < points.Count - 1; i++)
            {
                int index = i * 6;
                triangles[index] = i;
                triangles[index + 1] = i + 1;
                triangles[index + 2] = i;
                triangles[index + 3] = i + 1;
                triangles[index + 4] = i + 2;
                triangles[index + 5] = i + 1;
            }
            return triangles;
        }
    }
}