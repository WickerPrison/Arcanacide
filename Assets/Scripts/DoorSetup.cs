using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DoorSetup : MonoBehaviour
{
    [SerializeField] Transform openDoorMessage;
    [SerializeField] Transform notOpenMessage;
    [SerializeField] Transform wall;
    SortingGroup sortingGroup;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Vector3 offset;
    [SerializeField] SpriteRenderer fogWall;
    int direction = 1;

    private void OnDrawGizmosSelected()
    {
        if(wall != null)
        {
            transform.localPosition = wall.localPosition + offset;
            transform.localRotation = Quaternion.Euler(0, wall.localEulerAngles.y, 0);
            sortingGroup = wall.GetComponent<SortingGroup>();
            spriteRenderer.sortingLayerName = sortingGroup.sortingLayerName;
            fogWall.sortingLayerName = sortingGroup.sortingLayerName;
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
