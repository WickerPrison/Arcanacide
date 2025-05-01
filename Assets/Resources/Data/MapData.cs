using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireSuppressionState
{
    ON, OFF, FIXED
}

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public Color floorColor;
    public int floor;

    public int doorNumber;

    public List<string> deadEnemies;

    public List<int> unlockedDoors;

    public List<int> visitedRooms;

    public int currentRoom;

    public List<int> usedCoolers;

    public FireSuppressionState fireSuppressionState;

    public List<int> powerSwitchesFlipped;

    public string deathRoom;
    public Vector3 deathPosition;
    public bool ticketFiled;
    public bool miniboss1Killed;
    public bool fireBossKilled;
    public bool secretaryConvo;

    public bool electricBossKilled;
    public List<string> carolsDeadFriends;

    public bool iceBossKilled;
    public Vector3 iceBossPosition;
    public int iceBossDirection;

    public bool ACOn;

    public bool whistleblowerArrested;

    private void OnEnable()
    {
        doorNumber = 0;
        deadEnemies.Clear();
        unlockedDoors.Clear();
        usedCoolers.Clear();
        fireBossKilled = false;
    }
}
