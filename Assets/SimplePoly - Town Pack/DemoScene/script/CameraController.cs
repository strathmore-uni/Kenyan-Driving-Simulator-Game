using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Rigidbody playerRB;
    public Vector3 offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
     playerRB = player.GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerForward = (playerRB.velocity+ player.transform.forward ).normalized;
        transform.position = Vector3.Lerp(transform.position, player.position + player.transform.TransformVector(offset)+playerForward * (-5f), speed * Time.deltaTime);
        transform.LookAt(player);
    }
}
