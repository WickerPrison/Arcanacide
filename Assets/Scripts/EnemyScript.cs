using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //this script controls the automatic workings of the enemy like health 

    [System.NonSerialized] public int health;
    [System.NonSerialized] public float poise;
    public int reward;
    public GameObject healthbar;
    [SerializeField] float healthbarScale;
    public int enemyID;
    public MapData mapData;
    public PlayerData playerData;
    [SerializeField] DialogueData phoneData;
    public EmblemLibrary emblemLibrary;
    EnemyController enemyController;
    EnemyEvents enemyEvents;
    EnemySound enemySound;
    GameManager gm;
    public int maxHealth;
    [SerializeField] float maxPoise;
    [SerializeField] float poiseRegeneration;
    float staggerDuration = 2;
    [System.NonSerialized] public float DOT = 0;
    [System.NonSerialized] public bool blockAttack = false;
    float damageDOT = 0;
    public bool invincible = false;
    [System.NonSerialized] public bool dying = false;

    private void Awake()
    {
        if (mapData.deadEnemies.Contains(enemyID))
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        poise = maxPoise;
        enemyController = GetComponent<EnemyController>();
        enemyEvents = GetComponent<EnemyEvents>();
        enemySound = GetComponentInChildren<EnemySound>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.enemies.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(poise < maxPoise)
        {
            poise += Time.deltaTime * poiseRegeneration;
        }

        if(health <= 0 && enemyController.state != EnemyState.DYING)
        {
            dying = true;
            enemyController.state = EnemyState.DYING;
            enemyEvents.StopDOT();
            enemyController.StartDying();
        }

        if(DOT > 0 && enemyController.state != EnemyState.DYING)
        {
            DOT -= Time.deltaTime;
            damageDOT += Time.deltaTime * (playerData.arcane / 2 + 2);
            if(damageDOT >= 1)
            {
                LoseHealth(1, 0); ;
                damageDOT -= 1;
            }

            if(DOT <= 0)
            {
                DOT = 0;
                enemyEvents.StopDOT();
            }
        }
    }

    public void LoseHealth(int damage, float poiseDamage)
    {
        if (invincible)
        {
            if (poiseDamage > 0) enemyEvents.HitWhileInvincible();
            return;
        }
        if (enemyController.state == EnemyState.DYING) return;

        enemyEvents.TakeDamage();
        health -= damage;
        if(health < 0)
        {
            health = 0;
        }
        LosePoise(poiseDamage);
        UpdateHealthbar();
    }

    public void ImpactVFX()
    {
        enemyEvents.AttackImpact();
    }

    public void GainDOT(float duration)
    {
        if(duration > 0 && duration > DOT)
        {
            DOT = duration;
            enemyEvents.StartDOT();
        }
    }

    public void GainHealth(int healAmount)
    {
        if (enemyController.state == EnemyState.DYING) return;
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealthbar();
    }

    public void LosePoise(float poiseDamage)
    {
        if(poiseDamage <= 0 || health <= 0)
        {
            return;
        }
        enemyEvents.LosePoise();

        if (enemyController.state != EnemyState.ATTACKING && enemyController.state != EnemyState.DYING)
        {
            StartStagger(0.2f);
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.heavy_blows))
        {
            poiseDamage *= 1.5f;
        }
        poise -= poiseDamage;

        if (poise <= 0)
        {
            StartStagger(staggerDuration);
            poise = maxPoise;
        }
    }

    public void StartStagger(float staggerDuration)
    {
        enemyEvents.Stagger();
        enemyController.StartStagger(staggerDuration);
    }

    public void UpdateHealthbar()
    {
        if (healthbar == null) return;
        float healthbarRatio = (float)health / (float)maxHealth;
        if(health > 0)
        {
            healthbar.transform.localScale = new Vector3(healthbarRatio * healthbarScale, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
        }
        else
        {
            healthbar.transform.localScale = new Vector3(0, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
        }
    }


    public void Death()
    {
        if(enemyID != 0)
        {
            mapData.deadEnemies.Add(enemyID);
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.pay_raise))
        {
            playerData.money += Mathf.RoundToInt(reward * 1.25f);
        }
        else
        {
            playerData.money += reward;
        }
        gm.enemies.Remove(this);
        gm.enemiesInRange.Remove(this);
        gm.awareEnemies -= 1;

        GlobalEvents.instance.EnemyKilled();

        enemyEvents.Death();

        Destroy(gameObject);
    }
}
