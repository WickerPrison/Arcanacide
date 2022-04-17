using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public int doorNumber;

    public List<int> deadEnemies;

    public List<int> unlockedDoors;

    public List<int> visitedRooms;

    public List<int> usedChargingStations;

    public string deathRoom;
    public Vector3 deathPosition;
    public bool ticketFiled;
    public bool fireBossKilled;
    public bool secretaryConvo;

    private void OnEnable()
    {
        doorNumber = 0;
        deadEnemies.Clear();
        unlockedDoors.Clear();
        usedChargingStations.Clear();
        fireBossKilled = false;
    }
}
