using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRoom : MonoBehaviour
{
    [SerializeField] MapData mapData;
    public int roomId;
    Image image;

    private void Start()
    {
        if (!mapData.visitedRooms.Contains(roomId))
        {
            gameObject.SetActive(false);
            return;
        }

        image = GetComponent<Image>();
        if(mapData.currentRoom == roomId)
        {
            image.color = mapData.floorColor;
        }
        else
        {
            image.color = Color.white;
        }
    }
}
