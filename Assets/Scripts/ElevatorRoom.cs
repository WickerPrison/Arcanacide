using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ElevatorRoom : MonoBehaviour, IBlockDoors
{
    public event EventHandler<bool> onBlockDoor;
    CameraFollow cameraFollow;
    float tripDuration = 5;

    private void Start()
    {
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        onBlockDoor?.Invoke(this, true);
        StartCoroutine(ElevatorTrip());
    }

    IEnumerator ElevatorTrip()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(cameraFollow.ScreenShake(tripDuration, 0.01f));
        yield return new WaitForSeconds(tripDuration);
        onBlockDoor?.Invoke(this, false);
    }
}
