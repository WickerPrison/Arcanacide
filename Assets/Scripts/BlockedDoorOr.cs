using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockedDoorOr : BlockedDoor
{
    [SerializeField] int blockageID2;

    public override bool IsOpen()
    {
        bool isOpen = false;
        switch (blockageType)
        {
            case BlockageType.LOCK:
                if (mapData.unlockedDoors.Contains(blockageID) || mapData.unlockedDoors.Contains(blockageID2)) isOpen = true;
                break;
            case BlockageType.SUPPORTTICKET:
                isOpen = mapData.ticketFiled;
                break;
            case BlockageType.ELECTRICPUDDLE:
                if (mapData.powerSwitchesFlipped.Contains(blockageID) || mapData.powerSwitchesFlipped.Contains(blockageID2)) isOpen = true;
                break;
        }
        return isOpen;
    }
}
