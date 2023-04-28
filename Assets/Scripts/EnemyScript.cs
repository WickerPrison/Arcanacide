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
    EnemySound enemySound;
    GameManager gm;
    public int maxHealth;
    [SerializeField] float maxPoise;
    [SerializeField] float poiseRegeneration;
    [SerializeField] float staggerDuration;
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] ParticleSystem dotVFX;
    [System.NonSerialized] public bool isDying = false;
    float DOT = 0;
    float damageDOT = 0;
    public bool invincible = false;

    public EventHandler OnTakeDamage;
    public EventHandler OnStagger;

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

        if(health <= 0 && !isDying)
        {
            isDying = true;
            dotVFX.Stop();
            dotVFX.Clear();
            enemyController.StartDying();
        }

        if(DOT > 0 && !isDying)
        {
            DOT -= Time.deltaTime;
            damageDOT += Time.deltaTime * playerData.dedication;
            if(damageDOT >= 1)
            {
                LoseHealth(1, 0); ;
                damageDOT -= 1;
            }

            if(DOT <= 0)
            {
                dotVFX.Stop();
            }
        }
    }

    public void LoseHealth(int damage, float poiseDamage)
    {
        if (invincible) return;
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
        hitVFX.Play();
        health -= damage;
        if(health < 0)
        {
            health = 0;
        }
        LosePoise(poiseDamage);
        UpdateHealthbar();
    }

    public void GainDOT(float duration)
    {
        if(duration > 0)
        {
            DOT += duration;
            dotVFX.Play();
        }
    }

    public void GainHealth(int healAmount)
    {
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
        enemyController.OnHit();

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
        OnStagger?.Invoke(this, EventArgs.Empty);
        enemyController.StartStagger(staggerDuration);
    }

    void UpdateHealthbar()
    {
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
        playerData.killedEnemiesNum += 1;

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

        if (playerData.equippedEmblems.Contains(emblemLibrary.vampiric_strikes))
        {
            PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            int healAmount = Mathf.FloorToInt(playerData.MaxHealth() / 5);
            playerScript.PartialHeal(healAmount);
        }

        Destroy(gameObject);
    }
}
