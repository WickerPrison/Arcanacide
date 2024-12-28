using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayIfDemo : MonoBehaviour
{
    [SerializeField] DemoMode demoMode;
    Doorway doorway;
    public string nextRoom;
    public int doorNumber;
    public int lockedDoorID;

    private void Start()
    {
        if (!demoMode.demoMode) return;
        doorway = GetComponent<Doorway>();
        doorway.nextRoom = nextRoom;
        doorway.doorNumber = doorNumber;
        doorway.lockedDoorID = lockedDoorID;
    }
}
