using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAnimationEvents : EnemyAnimationEvents
{
    BossController bossController;

    public override void Start()
    {
        base.Start();
        bossController = GetComponentInParent<BossController>();
    }

    public void FireBlast()
    {
        bossController.StartFireBlast();
    }

    public void FireTral()
    {
        bossController.FireTrail();
    }

    public override void EnableMovement()
    {
        base.EnableMovement();
        int leftOrRight = Random.Range(1, 3);
        if(leftOrRight == 1)
        {
            bossController.strafeLeftOrRight *= -1;
        }
    }
}
