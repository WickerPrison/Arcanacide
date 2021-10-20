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
}
