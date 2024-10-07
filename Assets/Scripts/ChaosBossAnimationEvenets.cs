using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossSummons
{
    FATMAN
}

[System.Serializable]
public class ChaosBossAnimationEvenets : EnemyAnimationEvents
{
    FacePlayer facePlayer;
    [SerializeField] ChaosBossController chaosBossController;

    public override void Start()
    {
        base.Start();
        facePlayer = GetComponentInParent<FacePlayer>();
    }

    public void TurnTowardsPlayer()
    {
        facePlayer.ResetDestination();
        facePlayer.ManualFace();
    }

    public void Summon(BossSummons summon)
    {
        if(summon == BossSummons.FATMAN)
        {
            SummonFatMan();
        }
    }

    void SummonFatMan()
    {
        chaosBossController.FatManAttack();
    }
}
