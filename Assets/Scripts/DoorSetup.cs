using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class DoorSetup : MonoBehaviour
{
    [SerializeField] Transform openDoorMessage;
    [SerializeField] Transform notOpenMessage;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer fogWall;
    public Transform spawnPoint;
    [SerializeField] Vector3 offset;

    RoomSetupScript roomSetup;
    float spawnPointDistance = 1.5f;

    enum DoorDirection
    {
        LEFT, RIGHT, UP, DOWN
    }
    [SerializeField] DoorDirection direction;

#if UNITY_EDITOR
    private void OnEnable()
    {
        roomSetup = FindObjectOfType<RoomSetupScript>();
        roomSetup.onSizeChange += OnSizeChange;
    }

    private void OnSizeChange(object sender, System.EventArgs e)
    {
        if(roomSetup == null) return;
        Object[] objects = { transform, spriteRenderer, fogWall, spawnPoint, openDoorMessage, notOpenMessage };
        Undo.RecordObjects(objects, "Resize Room");
        UpdateDoor();
    }

    private void OnValidate()
    {
        if (roomSetup == null) return;
        Object[] objects = { transform, spriteRenderer, fogWall, spawnPoint, openDoorMessage, notOpenMessage };
        Undo.RecordObjects(objects, "Update Door");
        UpdateDoor();
    }

    private void UpdateDoor()
    {
        switch (direction)
        {
            case DoorDirection.LEFT:
                transform.localPosition = roomSetup.transform.localPosition + new Vector3(0, 1.65f, roomSetup.roomSize.y * 5) + offset;
                transform.localEulerAngles = Vector3.zero;
                spriteRenderer.sortingLayerName = "Floor";
                fogWall.sortingLayerName = "Floor";
                spawnPoint.localPosition = new Vector3(0, 0, -spawnPointDistance);
                openDoorMessage.localRotation = Quaternion.Euler(30, 45, openDoorMessage.rotation.z);
                notOpenMessage.localRotation = Quaternion.Euler(30, 45, notOpenMessage.localEulerAngles.z);
                break;
            case DoorDirection.RIGHT:
                transform.localPosition = roomSetup.transform.localPosition + new Vector3(0, 1.65f, -roomSetup.roomSize.y * 5) + offset;
                transform.localEulerAngles = Vector3.zero;
                spriteRenderer.sortingLayerName = "Foreground";
                fogWall.sortingLayerName = "Foreground";
                spawnPoint.localPosition = new Vector3(0, 0, spawnPointDistance);
                openDoorMessage.localRotation = Quaternion.Euler(30, 45, openDoorMessage.rotation.z);
                notOpenMessage.localRotation = Quaternion.Euler(30, 45, notOpenMessage.localEulerAngles.z);
                break;
            case DoorDirection.UP:
                transform.localPosition = roomSetup.transform.localPosition + new Vector3(roomSetup.roomSize.x * 5, 1.65f, 0) + offset;
                transform.localEulerAngles = new Vector3(0, 90, 0);
                spriteRenderer.sortingLayerName = "Floor";
                fogWall.sortingLayerName = "Floor";
                spawnPoint.localPosition = new Vector3(0, 0, -spawnPointDistance);
                openDoorMessage.localRotation = Quaternion.Euler(30, -45, openDoorMessage.rotation.z);
                notOpenMessage.localRotation = Quaternion.Euler(30, -45, notOpenMessage.localEulerAngles.z);
                break;
            case DoorDirection.DOWN:
                transform.localPosition = roomSetup.transform.localPosition + new Vector3(-roomSetup.roomSize.x * 5, 1.65f, 0) + offset;
                transform.localEulerAngles = new Vector3(0, 90, 0);
                spriteRenderer.sortingLayerName = "Foreground";
                fogWall.sortingLayerName = "Foreground";
                spawnPoint.localPosition = new Vector3(0, 0, spawnPointDistance);
                openDoorMessage.localRotation = Quaternion.Euler(30, -45, openDoorMessage.rotation.z);
                notOpenMessage.localRotation = Quaternion.Euler(30, -45, notOpenMessage.localEulerAngles.z);
                break;
        }
    }

    private void OnDisable()
    {
        roomSetup.onSizeChange -= OnSizeChange;
    }
#endif
}