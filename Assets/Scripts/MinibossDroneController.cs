using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DroneState
{
    IDLE
}

public class MinibossDroneController : MonoBehaviour
{
    [SerializeField] int droneId;
    DroneState droneState = DroneState.IDLE;
    Vector3 focusPoint;
    PlayerScript playerScript;
    FaceDirection faceDirection;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        faceDirection = GetComponent<FaceDirection>();
    }

    void Update()
    {
        switch (droneState)
        {
            case DroneState.IDLE:
                focusPoint = playerScript.transform.position;
                faceDirection.FaceTowards(focusPoint);
                break;
        }
    }
}
