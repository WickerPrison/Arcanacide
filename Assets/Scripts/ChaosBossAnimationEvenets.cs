using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChaosBossAnimationEvenets : EnemyAnimationEvents
{
    FacePlayer facePlayer;

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
}
