using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockedDoor : MonoBehaviour
{
    public enum BlockageType
    {
        LOCK, SUPPORTTICKET, ELECTRICPUDDLE, ICEBLOCK
    }

    public BlockageType blockageType;
    public int blockageID;
    public List<int> adjacentRooms = new List<int>();
    public List<Image> x = new List<Image>();
    public MapData mapData;
    private void OnEnable()
    {
        SetIcon();
    }

    void SetIcon()
    {
        SetX(!IsOpen());
        int adjacencyNum = 0;
        foreach (int room in adjacentRooms)
        {
            if (mapData.visitedRooms.Contains(room))
            {
                adjacencyNum++;
            }
        }

        if (adjacencyNum == 0) SetX(false);
    }

    public virtual bool IsOpen()
    {
        bool isOpen = false;
        switch(blockageType)
        {
            case BlockageType.LOCK:
                isOpen = mapData.unlockedDoors.Contains(blockageID);
                break;
            case BlockageType.SUPPORTTICKET:
                isOpen = mapData.ticketFiled;
                break;
            case BlockageType.ELECTRICPUDDLE:
                isOpen = mapData.powerSwitchesFlipped.Contains(blockageID);
                break;
            case BlockageType.ICEBLOCK:
                isOpen = !mapData.ACOn;
                break;
        }
        return isOpen;
    }
    
    void SetX(bool xOn)
    {
        foreach(Image image in x)
        {
            image.enabled = xOn;
        }
    }

}
