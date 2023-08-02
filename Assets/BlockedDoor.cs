using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockedDoor : MonoBehaviour
{
    private enum BlockageType
    {
        LOCK, SUPPORTTICKET
    }

    [SerializeField] BlockageType blockageType;
    [SerializeField] int blockageID;
    [SerializeField] List<int> adjacentRooms = new List<int>();
    [SerializeField] List<Image> x = new List<Image>();
    [SerializeField] MapData mapData;
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

    bool IsOpen()
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
