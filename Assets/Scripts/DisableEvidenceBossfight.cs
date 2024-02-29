using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEvidenceBossfight : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] CollectEvidence collectEvidence;
    [SerializeField] ParticleSystem wayFaerie;

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
                wayFaerie.gameObject.SetActive(mapData.electricBossKilled);
                collectEvidence.enabled = mapData.electricBossKilled;
                break;
            case Boss.FRANK:
                wayFaerie.gameObject.SetActive(mapData.iceBossKilled);
                collectEvidence.enabled = mapData.iceBossKilled; 
                break;
        }
    }
}
