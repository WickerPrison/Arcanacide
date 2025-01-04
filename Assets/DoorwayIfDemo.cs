using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayIfDemo : MonoBehaviour
{
    [SerializeField] BuildMode buildMode;
    Doorway doorway;
    public string nextRoom;
    public int doorNumber;
    public int lockedDoorID;

    private void Start()
    {
        if (buildMode.buildMode != BuildModes.DEMO) return;
        doorway = GetComponent<Doorway>();
        doorway.nextRoom = nextRoom;
        doorway.doorNumber = doorNumber;
        doorway.lockedDoorID = lockedDoorID;
    }
}
