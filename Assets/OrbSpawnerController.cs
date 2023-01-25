using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrbSpawnerController : EnemyController
{
    [SerializeField] GameObject lightningOrbPrefab;
    [SerializeField] Transform spawnPoint;
    float maxSpawnTime = 5;
    float spawnTimer = 0;

    public override void Start()
    {
        base.Start();

        spawnTimer = maxSpawnTime;
    }

    public override void Update()
    {
        if (enemyScript.isDying || isDisabled)
        {
            return;
        }

        EnemyAI();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();


        if(spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer <= 0)
            {
                spawnTimer = maxSpawnTime;
                SpawnOrb();
            }
        }
    }

    void SpawnOrb()
    {
        LightningOrbController orb = Instantiate(lightningOrbPrefab).GetComponent<LightningOrbController>();
        orb.transform.position = spawnPoint.position;
    }

    public override void StartDying()
    {
        enemyScript.Death();
    }

    public override void OnHit()
    {

    }

    public override void StartStagger(float staggerDuration)
    {
        
    }
}
