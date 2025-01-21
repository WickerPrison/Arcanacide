using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class WallSetup : MonoBehaviour
{
#if UNITY_EDITOR
    RoomSetupScript RoomSetup;
    RoomSetupScript roomSetup
    {
        get
        {
            if(RoomSetup == null)
            {
                RoomSetup = GetComponentInParent<RoomSetupScript>();
            }
            return RoomSetup;
        }
    }
    enum WallDirection
    {
        LEFT, RIGHT, UP, DOWN
    }
    [SerializeField] WallDirection direction;
    Vector3 startPosition;

    private void OnEnable()
    {
        roomSetup.onSizeChange += OnSizeChange;
        startPosition = new Vector3(roomSetup.transform.localPosition.x, roomSetup.transform.localPosition.y + 2.5f, roomSetup.transform.localPosition.z);
    }

    private void OnSizeChange(object sender, System.EventArgs e)
    {
        Undo.RecordObject(transform, "Resize Room");
        switch (direction)
        {
            case WallDirection.LEFT:
                transform.localScale = new Vector3(roomSetup.roomSize.x, 0.5f, 0.5f);
                transform.localPosition = startPosition + new Vector3(0, 0, roomSetup.roomSize.y * 5);
                transform.localEulerAngles = new Vector3(-90, 0, 0);
                break;
            case WallDirection.RIGHT:
                transform.localScale = new Vector3(roomSetup.roomSize.x, 0.5f, 0.5f);
                transform.localPosition = startPosition + new Vector3(0, 0, -roomSetup.roomSize.y * 5);
                transform.localEulerAngles = new Vector3(-90, 0, 0);
                break;
            case WallDirection.UP:
                transform.localScale = new Vector3(roomSetup.roomSize.y, 0.5f, 0.5f);
                transform.localPosition = startPosition + new Vector3(roomSetup.roomSize.x * 5, 0, 0);
                transform.localEulerAngles = new Vector3(-90, 90, 0);
                break;
            case WallDirection.DOWN:
                transform.localScale = new Vector3(roomSetup.roomSize.y, 0.5f, 0.5f);
                transform.localPosition = startPosition + new Vector3(-roomSetup.roomSize.x * 5, 0, 0);
                transform.localEulerAngles = new Vector3(-90, 90, 0);
                break;
        }
    }

    private void OnDisable()
    {
        roomSetup.onSizeChange -= OnSizeChange;
    }
#endif
}