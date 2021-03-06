using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //this script controls the automatic workings of the enemy like health 

    public int health;
    public float poise;
    [SerializeField] int reward;
    [SerializeField] GameObject healthbar;
    [SerializeField] float healthbarScale;
    [SerializeField] int enemyID;
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    EnemyController enemyController;
    EnemySound enemySound;
    GameManager gm;
    [SerializeField] int maxHealth;
    [SerializeField] float maxPoise;
    [SerializeField] float poiseRegeneration;
    [SerializeField] bool isBoss;
    [SerializeField] float staggerDuration;
    [SerializeField] ParticleSystem hitVFX;
    Animator frontAnimator;
    Animator backAnimator;
    public bool isDying = false;

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
        enemyController = GetComponent<EnemyController>();
        enemySound = GetComponentInChildren<EnemySound>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.enemies.Add(this);
        frontAnimator = enemyController.frontAnimator;
        backAnimator = enemyController.backAnimator;
    }

    public void LoseHealth(int damage, float poiseDamage)
    {
        hitVFX.Play();
        health -= damage;
        LosePoise(poiseDamage);
        UpdateHealthbar();
        enemyController.OnHit();
    }

    public void LosePoise(float poiseDamage)
    {
        if(poiseDamage <= 0 || health <= 0)
        {
            return;
        }

        if (!enemyController.attacking && !isDying)
        {
            enemyController.StartStagger(0.2f);
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.heavy_blows))
        {
            poiseDamage *= 1.5f;
        }
        poise -= poiseDamage;
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
            healthbar.transform.localScale = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(poise <= 0)
        {
            enemyController.StartStagger(staggerDuration);
            poise = maxPoise;
        }

        if(poise < maxPoise)
        {
            poise += Time.deltaTime * poiseRegeneration;
        }

        if(health <= 0 && !isDying)
        {
            isDying = true;
            frontAnimator.Play("Death");
            backAnimator.Play("Death");
        }
    }

    public void Death()
    {
        mapData.deadEnemies.Add(enemyID);
        if (playerData.equippedEmblems.Contains(emblemLibrary.pay_raise))
        {
            playerData.money += Mathf.RoundToInt(reward * 1.25f);
        }
        else
        {
            playerData.money += reward;
        }
        gm.enemies.Remove(this);
        if (enemyController.detectionTrigger)
        {
            gm.awareEnemies -= 1;
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.vampiric_strikes))
        {
            PlayerScript playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            int healAmount = Mathf.FloorToInt(playerData.MaxHealth() / 5);
            playerScript.PartialHeal(healAmount);
        }

        if (isBoss)
        {
            gm.awareEnemies -= 1;
            GameObject bossHealthbar = healthbar.transform.parent.gameObject;
            bossHealthbar.SetActive(false);
            ManagerVanquished managerVanquished = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<ManagerVanquished>();
            managerVanquished.ShowMessage();
            SoundManager sm = gm.gameObject.GetComponent<SoundManager>();
            sm.BossDefeated();
            MusicManager musicManager = gm.GetComponentInChildren<MusicManager>();
            musicManager.StartFadeOut(4);
        }

        Destroy(gameObject);
    }
}
