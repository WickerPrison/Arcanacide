using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElectricMageController : EnemyController
{
    [SerializeField] GameObject LightningBoltPrefab;
    List<LightningBolt> lightningBolts = new List<LightningBolt>();
    List<ElectricAlly> otherEnemies = new List<ElectricAlly>();
    ElectricAlly target = null;
    Vector3 away = new Vector3(100, 100, 100);
    int shieldDamage = 50;
    float shieldPoiseDamage = 70;
    LayerMask layerMask;
    float boltCD = 0;
    float boltMaxCD = 1;
    int boltDamage = 20;
    float boltPoiseDamage = 40;
    float strafeSpeed = 3;
    int strafeLeftOrRight = 1;
    float strafeTimer;

    public override void Start()
    {
        base.Start();
        for(int i = 0; i < 3; i++)
        {
            lightningBolts.Add(Instantiate(LightningBoltPrefab).GetComponent<LightningBolt>());
            lightningBolts[i].frameCounter = i;
        }
        BoltAway();

        CreateOtherEnemies();
        foreach(ElectricAlly enemy in otherEnemies)
        {
            enemy.damage = shieldDamage;
            enemy.poiseDamage = shieldPoiseDamage;
        }

        layerMask = LayerMask.GetMask("Player");

        int plusOrMinus = Random.Range(0, 2);
        if(plusOrMinus == 0)
        {
            strafeLeftOrRight *= -1;
        }

        strafeTimer = Random.Range(2f, 10f);
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (hasSeenPlayer)
        {
            CreateOtherEnemies();

            if (otherEnemies.Count > 0)
            {
                target = otherEnemies[0];
                foreach(ElectricAlly enemy in otherEnemies)
                {
                    enemy.isShielded = false;
                    if(enemy.priorityValue > target.priorityValue)
                    {
                        target = enemy;
                    }
                }
                target.isShielded = true;
                MovementAI();
            }
            else
            {
                BoltAway();
            }
        }
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            foreach(LightningBolt bolt in lightningBolts)
            {
                bolt.SetPositions(transform.position, target.transform.position);
            }

            BoltDamage();
        }
    }

    void BoltDamage()
    {
        if(boltCD > 0)
        {
            boltCD -= Time.fixedDeltaTime;
            return;
        }

        Vector3 direction = target.transform.position - transform.position;
        float distance = Vector3.Distance(target.transform.position, transform.position);
        bool playerHit = Physics.Raycast(transform.position, direction, distance, layerMask);
        if (playerHit)
        {
            playerScript.LoseHealth(boltDamage);
            playerScript.LosePoise(boltPoiseDamage);
            boltCD = boltMaxCD;
        }
    }

    void BoltAway()
    {
        foreach(LightningBolt bolt in lightningBolts)
        {
            bolt.SetPositions(away, away);
        }
    }

    void CreateOtherEnemies()
    {
        otherEnemies.Clear();
        for (int enemy = 0; enemy < gm.enemies.Count; enemy++)
        {
            if (gm.enemies[enemy] != enemyScript)
            {
                otherEnemies.Add(gm.enemies[enemy].GetComponent<ElectricAlly>());
            }
        }
    }

    void MovementAI()
    {
        Strafe();

        if (Vector3.Distance(playerController.transform.position, transform.position) < 5)
        {
            Vector3 awayFromPlayer = transform.position - playerController.transform.position;
            navAgent.Move(awayFromPlayer.normalized * Time.fixedDeltaTime * 2);
        }
    }

    void Strafe()
    {
        if(strafeTimer > 0)
        {
            strafeTimer -= Time.deltaTime;
            if(strafeTimer <= 0)
            {
                strafeTimer = Random.Range(2f, 10f);
            }
        }
        Vector3 playerToenemy = transform.position - playerController.transform.position;
        playerToenemy *= strafeLeftOrRight;
        Vector3 strafeDirection = Vector3.Cross(transform.position, playerToenemy);
        navAgent.Move(strafeDirection.normalized * Time.deltaTime * strafeSpeed);
    }
}
