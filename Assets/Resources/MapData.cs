using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapData : ScriptableObject
{
    public int doorNumber;

    public List<int> deadEnemies;

    private void OnEnable()
    {
        doorNumber = 0;
        deadEnemies.Clear();
    }
}
