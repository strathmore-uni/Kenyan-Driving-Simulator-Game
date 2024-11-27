using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    public Transform[] waypoints;  // Array to hold the waypoints
    public float speed = 5f;       // Speed of the object
    private int currentWaypointIndex = 0; // Current waypoint the object is moving towards

    void Update()
    {
        if (waypoints.Length == 0) return;

        // Move towards the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Check if the object has reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;  // Loops back to the start
        }
    }
}
