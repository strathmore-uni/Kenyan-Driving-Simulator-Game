using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_AI : MonoBehaviour
{
    public float safeDistance = 2f;
    public float carSpeed = 5f;

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, safeDistance);

        if(hit.transform)
        {
            if(hit.transform.tag == "Car")
            {
                Stop();
            }
            else
            {
                Move();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + safeDistance));
    }

    private void Stop()
    {
        transform.position += new Vector3(0, 0, 0);
    }
    private void Move()
    {
        transform.position += new Vector3(0, 0, carSpeed * Time.deltaTime);
    }
}
