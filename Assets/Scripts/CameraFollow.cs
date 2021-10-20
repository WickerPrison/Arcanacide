using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //This script makes the camera follow the player. Some delay is added to prevent jerking the camera when the player dashes.

    [SerializeField] GameObject movePoint;
    [SerializeField] PlayerController playerMovement;
    Vector3 offset = new Vector3(-0.4f, 5.9f, -7.7f);

    // Start is called before the first frame update
    void Start()
    {
        //break the parent/child relationship of the camera and the movePoint so they can move independently
        movePoint.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        //the movePoint perfectly copies the movement of the player
        movePoint.transform.position = playerMovement.transform.position + offset;
        transform.position = movePoint.transform.position;
    }

    private void FixedUpdate()
    {
        //the camera always moves towards the movePoint at a steady speed
        transform.position = Vector3.MoveTowards(transform.position, movePoint.transform.position, playerMovement.moveSpeed * 2 * Time.fixedDeltaTime);
    }
}
