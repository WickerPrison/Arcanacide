using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public int killedEnemies;
    public int killedEnemiesAtGainBlock;
    public int totalDeaths;

    public void ClearData()
    {
        killedEnemies = 0;
        killedEnemiesAtGainBlock = 0;
        totalDeaths = 0;
    }
}
