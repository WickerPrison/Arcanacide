using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapDoorDirection
{
    UP, DOWN, LEFT, RIGHT
}

#if UNITY_EDITOR
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

    private void OnEnable()
    {
        RoomSetup.doors.Add(this);
    }

    private void OnDisable()
    {
        RoomSetup.doors.Remove(this);
    }
}
#endif
