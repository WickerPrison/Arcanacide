using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEvidenceCarol : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] CollectEvidence collectEvidence;

    // Update is called once per frame
    void Update()
    {
        collectEvidence.enabled = mapData.electricBossKilled;
    }
}
