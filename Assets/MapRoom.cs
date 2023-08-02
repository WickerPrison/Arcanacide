using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoom : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] int roomId;

    private void Start()
    {
        if(!mapData.visitedRooms.Contains(roomId)) gameObject.SetActive(false);
    }
}
