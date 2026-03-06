using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EvidenceFloor
{
    FIRST, SECOND, THIRD
}

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public int killedEnemies;
    public int killedEnemiesAtGainBlock;
    public int totalDeaths;
    public Dictionary<EnemyType, int> deathsToEnemies = new Dictionary<EnemyType, int>();
    [SerializeField] List<EnemyType> deathsToEnemiesKeys;
    [SerializeField] List<int> deathsToEnemiesValues;
    public int firstFloorEvidence;
    public int secondFloorEvidence;
    public int thirdFloorEvidence;

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

    public int IncrementEvidenceForFloor(EvidenceFloor evidenceFloor)
    {
        switch (evidenceFloor)
        {
            case EvidenceFloor.FIRST:
                firstFloorEvidence++;
                return firstFloorEvidence;
            case EvidenceFloor.SECOND:
                secondFloorEvidence++;
                return secondFloorEvidence;
            case EvidenceFloor.THIRD:
                thirdFloorEvidence++;
                return thirdFloorEvidence;
            default:
                return -1;
        }
    }

    public int GetTotalEvidence()
    {
        return firstFloorEvidence + secondFloorEvidence + thirdFloorEvidence;
    }

    public void ClearData()
    {
        killedEnemies = 0;
        killedEnemiesAtGainBlock = 0;
        totalDeaths = 0;
        deathsToEnemies = new Dictionary<EnemyType, int>();
        deathsToEnemiesKeys = new List<EnemyType>();
        deathsToEnemiesValues = new List<int>();
        firstFloorEvidence = 0;
        secondFloorEvidence = 0;
        thirdFloorEvidence = 0;
    }
}
