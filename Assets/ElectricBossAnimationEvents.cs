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
        LightningOrbController orb = Instantiate(lightningOrbPrefab).GetComponent<LightningOrbController>();
        float x = Random.Range(-spawnRadius, spawnRadius);
        float z = Random.Range(-spawnRadius, spawnRadius);
        orb.transform.position = bossController.transform.position + new Vector3(x, 0, z).normalized;
    }

    public void StartCharge()
    {
        bossController.StartCharge();
    }
}
