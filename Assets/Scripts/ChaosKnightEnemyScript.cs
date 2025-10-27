using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosKnightEnemyScript : EnemyScript
{
    public bool blocking = false;
    ChaosKnightController chaosKnightController;

    public override void Start()
    {
        base.Start();
        chaosKnightController = GetComponent<ChaosKnightController>();
    }

    public override void LoseHealth(int damage, float poiseDamage, IDamageEnemy damageEnemy, Action unblockedCallback)
    {
        if (!blocking || !damageEnemy.blockable || !chaosKnightController.AttackerBlockableAngle(damageEnemy.transform))
        {
            LoseHealthUnblockable(damage, poiseDamage);
            unblockedCallback();
        }
        else
        {
            enemySound.BlockAttack();
        }
    }
}
