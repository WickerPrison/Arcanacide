using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaHarpoonEnemyScript : EnemyScript
{
    TeslaHarpoon teslaHarpoonController;

    public override void Start()
    {
        base.Start();
        teslaHarpoonController = GetComponent<TeslaHarpoon>();
    }

    public override void LoseHealth(int damage, float poiseDamage, IDamageEnemy damageEnemy, Action unblockedCallback)
    {
        teslaHarpoonController.DirecitonalAttack(damageEnemy.transform.position);
        base.LoseHealth(damage, poiseDamage, damageEnemy, unblockedCallback);
    }
}
