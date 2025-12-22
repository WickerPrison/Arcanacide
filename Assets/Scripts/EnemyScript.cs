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
    public MapData mapData;
    public PlayerData playerData;
    [SerializeField] DialogueData phoneData;
    public EmblemLibrary emblemLibrary;
    EnemyController enemyController;
    EnemyEvents enemyEvents;
    public EnemySound enemySound;
    GameManager gm;
    public int maxHealth;
    [SerializeField] float maxPoise;
    [SerializeField] float poiseRegeneration;
    float staggerDuration = 2;
    [System.NonSerialized] public float DOT = 0;
    float damageDOT = 0;
    public bool invincible = false;
    [System.NonSerialized] public bool dying = false;
    public string enemyGUID = "";
    [System.NonSerialized] public List<EnemyState> nonStaggerableStates = new List<EnemyState>();
    public int chargeResistance;
    int maxCharge = 10;
    [System.NonSerialized] public int electricCharge;
    float partialCharge;


    private void Awake()
    {
        if (mapData.deadEnemies.Contains(enemyGUID))
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        health = maxHealth;
        poise = maxPoise;
        maxCharge += chargeResistance;
        enemyController = GetComponent<EnemyController>();
        enemyEvents = GetComponent<EnemyEvents>();
        enemyEvents.UpdateHealth(1);
        enemySound = GetComponentInChildren<EnemySound>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.enemies.Add(this);
        nonStaggerableStates.Add(EnemyState.ATTACKING);
        nonStaggerableStates.Add(EnemyState.DYING);
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
                LoseHealthUnblockable(1, 0); ;
                damageDOT -= 1;
            }

            if(DOT <= 0)
            {
                DOT = 0;
                enemyEvents.StopDOT();
            }
        }
    }

    public virtual void LoseHealth(int damage, float poiseDamage, IDamageEnemy damageEnemy, Action unblockedCallback)
    {
        LoseHealthUnblockable(damage, poiseDamage);
        unblockedCallback();
    }

    public void LoseHealthUnblockable(int damage, float poiseDamage)
    {
        if (invincible)
        {
            if (poiseDamage > 0) enemyEvents.HitWhileInvincible();
            return;
        }
        if (enemyController.state == EnemyState.DYING) return;

        health -= damage;
        enemyEvents.TakeDamage();
        if (health < 0)
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

        if (!nonStaggerableStates.Contains(enemyController.state))
        {
            StartStagger(0.2f);
        }

        if (playerData.equippedPatches.Contains(Patches.HEAVY_BLOWS))
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

    public void GainElectricCharge(float amount)
    {
        partialCharge += amount;
        int charge = (int)partialCharge;
        partialCharge = partialCharge - charge;
        GainElectricCharge(charge);
    }

    public void GainElectricCharge(int amount)
    {
        electricCharge += amount;
        if(electricCharge >= maxCharge)
        {
            electricCharge = 0;
            maxCharge += 2 + Mathf.FloorToInt(chargeResistance / 2);
            enemyEvents.GetShocked();
            enemySound.ElectricShock();
            LoseHealthUnblockable(Mathf.FloorToInt(maxHealth * 0.1f) + 10, 50);
        }
    }

    public void StartStagger(float staggerDuration)
    {
        if (invincible) return;
        enemyController.StartStagger(staggerDuration);
    }

    public void UpdateHealthbar()
    {
        float healthbarRatio = (float)health / (float)maxHealth;
        enemyEvents.UpdateHealth(healthbarRatio);
    }


    public void Death()
    {
        if(enemyGUID != "")
        {
            mapData.deadEnemies.Add(enemyGUID);
        }

        if (playerData.equippedPatches.Contains(Patches.PAY_RAISE))
        {
            GlobalEvents.instance.MoneyChange(Mathf.RoundToInt(reward * (float)emblemLibrary.patchDictionary[Patches.PAY_RAISE].value));
        }
        else
        {
            GlobalEvents.instance.MoneyChange(reward);
        }
        gm.enemies.Remove(this);
        gm.enemiesInRange.Remove(this);
        gm.awareEnemies -= 1;

        GlobalEvents.instance.EnemyKilled();

        enemyEvents.Death();

        Destroy(gameObject);
    }

    public void GenerateGUID()
    {
        enemyGUID = System.Guid.NewGuid().ToString();
    }
}
