using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peque.Traffic
{
    public class VehicleController : MonoBehaviour
    {
        public float speed = 10; // speed of the vehicle
        public float waypointThreshold = 5; // distance from the vehicle to the waypoint

        private List<GameObject> waypoints; // list of waypoints
        private int currentWaypointIndex; // current waypoint index

        private void Start()
        {
            // get the list of waypoints from the road generator
            RoadGenerator roadGenerator = GameObject.FindObjectOfType<RoadGenerator>();
            waypoints = roadGenerator.waypoints;

            // set the initial waypoint index
            currentWaypointIndex = 0;
        }

        private void Update()
        {
            // follow the waypoints
            GameObject currentWaypointObject = waypoints[currentWaypointIndex];
            Waypoint currentWaypoint = currentWaypointObject.GetComponent<Waypoint>();
            float distanceToWaypoint = Vector3.Distance(transform.position, currentWaypoint.transform.position);
            if (distanceToWaypoint < waypointThreshold)
            {
                // reached the current waypoint, move to the next one
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }

            // move towards the current waypoint
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.transform.position, step);
        }
    }
}