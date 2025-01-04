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
                MapData = Resources.Load<MapData>("ScriptableObjects/MapData");
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
}
