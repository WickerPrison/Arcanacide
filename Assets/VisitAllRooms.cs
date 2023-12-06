using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitAllRooms : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] bool start = false;

    private void OnDrawGizmosSelected()
    {
        if (start)
        {
            start = false;
            mapData.visitedRooms.Clear();
            for(int i = -10; i < 150; i++)
            {
                mapData.visitedRooms.Add(i);
            }
        }

    }
}
