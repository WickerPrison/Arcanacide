using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] int roomNumber;
    [SerializeField] MapData mapData;
    public List<Transform> spawnPoints;
    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SpawnPosition();
        if (!mapData.visitedRooms.Contains(roomNumber))
        {
            mapData.visitedRooms.Add(roomNumber);
        }
    }

    public void SpawnPosition()
    {
        player.position = spawnPoints[mapData.doorNumber].position;
        player.position = new Vector3(player.position.x, 1, player.position.z);
    } 
}
