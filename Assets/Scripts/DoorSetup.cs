using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSetup : MonoBehaviour
{
    [SerializeField] Transform openDoorMessage;
    [SerializeField] Transform notOpenMessage;
    [SerializeField] Transform wall;
    [SerializeField] Vector3 offset;
    int direction = 1;

    private void OnDrawGizmosSelected()
    {
        if(wall != null)
        {
            transform.localPosition = wall.localPosition + offset;
            transform.localRotation = Quaternion.Euler(0, wall.localEulerAngles.y, 0);
        }
   
        if(transform.localEulerAngles.y == 90)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        openDoorMessage.localRotation = Quaternion.Euler(30, 45 * direction, openDoorMessage.rotation.z);
        notOpenMessage.localRotation = Quaternion.Euler(30, 45 * direction, notOpenMessage.localEulerAngles.z);
    }
}
