using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public Color floorColor;

    public int doorNumber;

    public List<int> deadEnemies;

    public List<int> unlockedDoors;

    public List<int> visitedRooms;

    public List<int> usedAltars;

    public List<int> powerSwitchesFlipped;

    public string deathRoom;
    public Vector3 deathPosition;
    public bool ticketFiled;
    public bool fireBossKilled;
    public bool secretaryConvo;

    public bool electricBossKilled;

    public bool iceBossKilled;
    public Vector3 iceBossPosition;
    public int iceBossDirection;

    public bool ACOn;

    private void OnEnable()
    {
        doorNumber = 0;
        deadEnemies.Clear();
        unlockedDoors.Clear();
        usedAltars.Clear();
        fireBossKilled = false;
    }
}