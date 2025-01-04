using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] int roomNumber;
    [SerializeField] MapData mapData;
    public List<Transform> spawnPoints;
    Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SpawnPosition();
        if (!mapData.visitedRooms.Contains(roomNumber))
        {
            mapData.visitedRooms.Add(roomNumber);
        }
        mapData.currentRoom = roomNumber;
    }

    public void SpawnPosition()
    {
        if(spawnPoints.Count > mapData.doorNumber)
        {
            player.position = spawnPoints[mapData.doorNumber].position;
        }
        else
        {
            player.position = spawnPoints[0].position;
        }
        player.position = new Vector3(player.position.x, 0, player.position.z);
    } 
}
