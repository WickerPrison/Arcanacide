using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElectricBossAnimationEvents : MeleeEnemyAnimationEvents
{
    [SerializeField] GameObject lightningOrbPrefab;
    [SerializeField] ParticleSystem swooshShock;
    ElectricBossController bossController;
    float spawnRadius = 1.3f;
    bool spawnOrb = true;

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

    public void SpawnOrb()
    {
        if (spawnOrb)
        {
            LightningOrbController orb = Instantiate(lightningOrbPrefab).GetComponent<LightningOrbController>();
            float x = Random.Range(-spawnRadius, spawnRadius);
            float z = Random.Range(-spawnRadius, spawnRadius);
            orb.transform.position = bossController.transform.position + new Vector3(x, 0, z).normalized;
        }

        if (!bossController.phase2)
        {
            spawnOrb = !spawnOrb;
        }
    }

    public void StartCharge()
    {
        bossController.StartCharge();
    }

    public override void Death()
    {
        base.Death();
        bossController.Death();
    }
}