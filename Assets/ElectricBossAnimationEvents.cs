using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElectricBossAnimationEvents : MeleeEnemyAnimationEvents
{
    [SerializeField] ParticleSystem swooshShock;
    ElectricBossController bossController;

    public override void Start()
    {
        base.Start();
        bossController = GetComponentInParent<ElectricBossController>();
    }

    public void SwingSword(int smearSpeed)
    {
        bossController.SwingSword(smearSpeed);
    }

    public void SwooshShock()
    {
        swooshShock.Play(true);
        bossController.canHitPlayer = attackArc.CanHitPlayer();
        bossController.SwooshShock();
    }

    public void Hadoken()
    {
        bossController.Hadoken();
    }
}
