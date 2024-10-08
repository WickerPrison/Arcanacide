using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FatmanSummon : ChaosSummon
{
    [SerializeField] int damage;
    [SerializeField] int poiseDamage;
    [System.NonSerialized] public AttackArcGenerator attackArcGenerator;
    [System.NonSerialized] public ChaosBossController bossController;

    public override void Awake()
    {
        base.Awake();
        attackArcGenerator = GetComponentInChildren<AttackArcGenerator>();
    }

    public override void Attack()
    {
        base.Attack();
        sfx.SwordSwoosh();
        sfx.OtherSounds(0, 2);
        attackArcGenerator.HideAttackArc();

        if (!attackArcGenerator.CanHitPlayer()) return;

        if (playerScript.gameObject.layer == 3)
        {
            sfx.SwordImpact();
            playerScript.LoseHealth(damage, EnemyAttackType.MELEE, enemyScript);
            playerScript.LosePoise(poiseDamage);
        }
        else if (playerScript.gameObject.layer == 8)
        {
            playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
        }
    }

    public override void ShowIndicator()
    {
        base.ShowIndicator();
        attackArcGenerator.ShowAttackArc();
    }

    public override void GoAway()
    {
        base.GoAway();
        bossController.fatMen.Enqueue(this);
    }
}
