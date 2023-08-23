using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapHallway : MonoBehaviour
{
    [SerializeField] MapRoom room1;
    [SerializeField] MapRoom room2;
    [SerializeField] MapData mapData;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();

        if(mapData.visitedRooms.Contains(room1.roomId) && mapData.visitedRooms.Contains(room2.roomId))
        {
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
    }
}
