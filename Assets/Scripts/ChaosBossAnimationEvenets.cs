using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossSummons
{
    FATMAN1, FATMAN2
}

[System.Serializable]
public class ChaosBossAnimationEvenets : EnemyAnimationEvents
{
    FacePlayer facePlayer;
    [SerializeField] ChaosBossController chaosBossController;
    FinalBossEvents events;

    public override void Start()
    {
        base.Start();
        events = GetComponentInParent<FinalBossEvents>();
        facePlayer = GetComponentInParent<FacePlayer>();
    }

    public void TurnTowardsPlayer()
    {
        facePlayer.ResetDestination();
        facePlayer.ManualFace();
    }

    public void Summon(BossSummons summon)
    {
        switch (summon)
        {
            case BossSummons.FATMAN1:
                SummonFatMan(2);
                break;
            case BossSummons.FATMAN2:
                SummonFatMan(-2);
                break;
        }

    }

    void SummonFatMan(float perpScale, float toPlayer = 1)
    {
        Vector2 perp = perpScale * Vector2.Perpendicular(new Vector2(facePlayer.faceDirection.x, facePlayer.faceDirection.z)).normalized;
        float xpos = perp.x + chaosBossController.transform.position.x + facePlayer.faceDirection.x / toPlayer;
        float zpos = perp.y + chaosBossController.transform.position.z + facePlayer.faceDirection.z / toPlayer;
        Vector3 position = new Vector3(xpos, 0, zpos);
        chaosBossController.FatManAttack(position, facePlayer.player.position - position);
    }

    public void ComboAssistant()
    {
        events.OnCombo();
    }
}
