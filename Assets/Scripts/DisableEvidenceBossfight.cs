using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEvidenceBossfight : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] CollectEvidence collectEvidence;

    enum Boss
    {
        CAROL, FRANK
    }

    [SerializeField] Boss boss;

    // Update is called once per frame
    void Update()
    {
        switch (boss)
        {
            case Boss.CAROL:
                collectEvidence.enabled = mapData.electricBossKilled;
                break;
            case Boss.FRANK:
                collectEvidence.enabled = mapData.iceBossKilled; 
                break;
        }
    }
}
