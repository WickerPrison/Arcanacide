using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOneWayDoor : MonoBehaviour
{
    [SerializeField] Doorway doorwayScript;
    [SerializeField] int lockedDoorID;
    [SerializeField] MapData mapData;
    Transform player;
    InputManager im;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => OpenDoor();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void OpenDoor()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2 && doorwayScript.doorOpen && !mapData.unlockedDoors.Contains(lockedDoorID))
        {
            mapData.unlockedDoors.Add(lockedDoorID);
        }
    }
}
