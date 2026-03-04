using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public int killedEnemies;
    public int killedEnemiesAtGainBlock;
    public int totalDeaths;
    public Dictionary<EnemyType, int> deathsToEnemies = new Dictionary<EnemyType, int>();
    [SerializeField] List<EnemyType> deathsToEnemiesKeys;
    [SerializeField] List<int> deathsToEnemiesValues;
    public int weaponsUnlocked;

    public int IncrementDeathsToEnemy(EnemyType type)
    {
        if (deathsToEnemies.ContainsKey(type))
        {
            deathsToEnemies[type]++;
        }
        else
        {
            deathsToEnemies.Add(type, 1);
        }
        deathsToEnemiesKeys = deathsToEnemies.Keys.ToList();
        deathsToEnemiesValues = deathsToEnemies.Values.ToList();
        return deathsToEnemies[type];
    }

    public void UnlockedWeapons(int count)
    {
        weaponsUnlocked = count;
    }

    public void ClearData()
    {
        killedEnemies = 0;
        killedEnemiesAtGainBlock = 0;
        totalDeaths = 0;
        deathsToEnemies = new Dictionary<EnemyType, int>();
        deathsToEnemiesKeys = new List<EnemyType>();
        deathsToEnemiesValues = new List<int>();
    }
}
