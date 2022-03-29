using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenOneWayDoor : MonoBehaviour
{
    [SerializeField] Doorway doorwayScript;
    [SerializeField] int lockedDoorID;
    [SerializeField] MapData mapData;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2 && doorwayScript.doorOpen && Input.GetKeyDown(KeyCode.E) && !mapData.unlockedDoors.Contains(lockedDoorID))
        {
            mapData.unlockedDoors.Add(lockedDoorID);
        }
    }
}
