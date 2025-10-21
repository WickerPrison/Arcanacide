using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class MapTools
{
    static MapData MapData;
    static MapData mapData
    {
        get
        {
            if(MapData == null)
            {
                MapData = Resources.Load<MapData>("Data/MapData");
            }
            return MapData;
        }
    }


    [MenuItem("Tools/Vist All Rooms")]
    public static void VisitAllRooms()
    {
        mapData.visitedRooms.Clear();
        mapData.visitedRooms = Enumerable.Range(-10, 150).ToList();
    }

    [MenuItem("Tools/Unvisit All Rooms")]
    public static void UnvisitAllRooms()
    {
        mapData.visitedRooms.Clear();
    }

    [MenuItem("Tools/Unlock All Doors")]
    public static void UnlockAllDoors()
    {
        mapData.unlockedDoors.Clear();
        mapData.unlockedDoors = Enumerable.Range(1, 15).ToList();
    }

    [MenuItem("Tools/Lock All Doors")]
    public static void LockAllDoors()
    {
        mapData.unlockedDoors.Clear();
    }

    [MenuItem("Tools/Respawn All Enemies")]
    public static void RespawnAllEnemies()
    {
        mapData.deadEnemies.Clear();
        mapData.miniboss1Killed = false;
        mapData.miniboss2Killed = false;
        mapData.miniboss3Killed = false;
        mapData.miniboss4Killed = false;
        mapData.fireBossKilled = false;
        mapData.electricBossKilled = false;
        mapData.iceBossKilled = false;
        mapData.carolsDeadFriends.Clear();
    }
}
