using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapDoorDirection
{
    UP, DOWN, LEFT, RIGHT
}

[ExecuteAlways]
public class MapDoorSetup : MonoBehaviour
{
    public MapDoorDirection direction;
    public float offset;
    MapRoomSetup roomSetup;
    MapRoomSetup RoomSetup
    {
        get
        {
            if(roomSetup == null) roomSetup = GetComponentInParent<MapRoomSetup>();
            return roomSetup;
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        RoomSetup.doors.Add(this);
    }

    private void OnDisable()
    {
        RoomSetup.doors.Remove(this);
    }
#endif
}
