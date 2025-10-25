using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[System.Serializable]
public class OrbSpawnerController : EnemyController
{
    [SerializeField] GameObject lightningOrbPrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform orbSprite;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] SpriteRenderer brokenSprite;
    SpriteEffects spriteEffects;
    float maxSpawnTime = 5;
    float spawnTimer = 0;
    Vector3 minSize = new Vector3(0.01f, 0.01f, 0.01f);
    Vector3 maxSize = Vector3.one;

    public override void Start()
    {
        base.Start();

        spawnTimer = maxSpawnTime;
        spriteEffects = GetComponent<SpriteEffects>();
    }

    public override void Update()
    {
        if (state == EnemyState.DYING || state == EnemyState.DISABLED)
        {
            return;
        }

        orbSprite.localScale = Vector3.Lerp(maxSize, minSize, spawnTimer / maxSpawnTime);

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
        if (spriteEffects.colorChange)
        {
            orb.colorChange = true;
        }
        // disabling and reenabling the navagent is a workaround for a bug in the navmesh that causes strange teleportations if you spawn an agent and then immediately set its position.
        orb.navAgent.enabled = false;
        orb.transform.position = spawnPoint.position;
        orb.navAgent.enabled = true;
    }

    public override void StartDying()
    {
        state = EnemyState.DYING;
        orbSprite.gameObject.SetActive(false);
        sprite.enabled = false;
        brokenSprite.enabled = true;
        StartCoroutine(DeathTimer());
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(spriteEffects.Dissolve());
        yield return new WaitForSeconds(1f);
        enemyScript.Death();
    }

    public override void StartStagger(float staggerDuration)
    {
        
    }

    public override void EnableController()
    {
        state = EnemyState.IDLE;
    }

    public override void DisableController()
    {
        if (state == EnemyState.DYING) return;
        if (state == EnemyState.UNAWARE)
        {
            gm.awareEnemies += 1;
        }
        state = EnemyState.DISABLED;
    }
}
