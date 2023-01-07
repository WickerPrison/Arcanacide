using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //This script is responsible for things that happen automatically without player input
    //such as health and stamina

    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] PlayerProjectile projectilePrefab;
    public float stamina;
    public float poise;
    SwordSiteDirectory swordSiteDirectory;
    GameManager gm;
    InputManager im;
    PlayerController playerController;
    PlayerAnimation playerAnimation;
    PlayerSound sfx;
    CameraFollow cameraScript;
    float maxStaminaDelay = 1f;
    float staminaDelay;
    float staminaRate = 40;
    float maxPoise = 100;
    float poiseRate = 5;
    float staggerTimer = 0;
    float poiseBreakStagger = 2;
    public bool isStaggered = false;
    public float duckHealTimer = 0;
    float duckHealDuration = 2;
    float duckHealCounter = 0;
    float duckHealSpeed = 0;
    public bool shield;
    public bool parry;

    float manaDelay;
    float maxManaDelay = 2;
    int blockManaCost = 15;
    float manaRechargeRate = 4;

    // Start is called before the first frame update
    void Start()
    {
        swordSiteDirectory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SwordSiteDirectory>();
        gm = swordSiteDirectory.gameObject.GetComponent<GameManager>();
        im = swordSiteDirectory.gameObject.GetComponent<InputManager>();
        stamina = playerData.MaxStamina();
        poise = maxPoise;
        playerController = GetComponent<PlayerController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        sfx = GetComponentInChildren<PlayerSound>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        if(playerData.equippedEmblems.Contains(emblemLibrary.arcane_step))
        {
            Physics.IgnoreLayerCollision(8, 6, true);
        }
    }

    public void LoseHealth(int damage, EnemyScript attackingEnemy = null)
    {
        if (!shield)
        {
            playerData.health -= damage;
            hitVFX.Play();
            float screenShakeMagnitude = (float)damage / (float)playerData.MaxHealth() * .1f;
            StartCoroutine(cameraScript.ScreenShake(.1f, screenShakeMagnitude));
            if(playerData.health <= 0)
            {
                im.DisableAll();
                playerController.preventInput = true;
                YouDied youDied = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<YouDied>();
                youDied.playerScript = this;
                youDied.ShowMessage();
                SoundManager sm = gm.gameObject.GetComponent<SoundManager>();
                sm.DeathSoundEffect();
                MusicManager musicManager = gm.gameObject.GetComponentInChildren<MusicManager>();
                musicManager.StartFadeOut(1);
            }
        }
        else
        {
            sfx.Shield();
            if (parry & attackingEnemy != null)
            {
                PlayerProjectile projectile = Instantiate(projectilePrefab).GetComponent<PlayerProjectile>();
                projectile.transform.position = transform.position;
                projectile.transform.LookAt(attackingEnemy.transform.position + new Vector3(0, 1.1f, 0));
                projectile.target = attackingEnemy.transform;
                projectile.playerController = playerController;
            }
        }
    }

    public void MaxHeal()
    {
        playerData.health = playerData.MaxHealth();
        sfx.Heal();
    }

    public void PartialHeal(int healAmount)
    {
        playerData.health += healAmount;
        if(playerData.health > playerData.MaxHealth())
        {
            playerData.health = playerData.MaxHealth();
        }
    }

    public void Heal()
    {
        if(playerData.healCharges >= 0)
        {
            if(playerData.unlockedAbilities.Count ==0 && playerData.healCharges == 0)
            {
                return;
            }
            playerData.healCharges -= 1;
            duckHealTimer = duckHealDuration;
            duckHealSpeed = playerData.MaxHealth() / duckHealDuration;
            sfx.Heal();
            playerAnimation.StartBodyMagic();
        }
    }

    public void LosePoise(float poiseDamage)
    {
        if (!shield)
        {
            poise -= poiseDamage;
            if(poise <= 0)
            {
                StartStagger(poiseBreakStagger);
                ResetPoise();
            }
        }
    }
    
    public void ResetPoise()
    {
        poise = maxPoise;
    }

    public void StartStagger(float staggerDuration)
    {
        if(staggerDuration > 0)
        {
            if (!playerController.knockback)
            {
                playerController.rb.velocity = Vector3.zero;
            }
            staggerTimer += staggerDuration;
            isStaggered = true;
            if (playerAnimation.animationSwordMagic)
            {
                playerAnimation.EndSwordMagic();
                playerAnimation.animationSwordMagic = false;
            }
            playerAnimation.PlayStagger();
        }
    }

    public void EndStagger()
    {
        staggerTimer = 0;
        isStaggered = false;
        playerAnimation.PlayIdle();
    }

    public void LoseStamina(float amount)
    {
        stamina -= amount;
        if(stamina < 0)
        {
            stamina = 0;
        }
        staminaDelay = maxStaminaDelay;
    }

    public void LoseMana(float amount)
    {
        playerData.mana -= amount;
        if(playerData.mana < 0)
        {
            playerData.mana = 0;
        }
        manaDelay = maxManaDelay;
        if (playerData.equippedEmblems.Contains("MagicalAcceleration"))
        {
            manaDelay = maxManaDelay / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAnimation.attacking)
        {
            if(staminaDelay > 0)
            {
                staminaDelay -= Time.deltaTime;
            }
            else if(stamina < playerData.MaxStamina())
            {
                stamina += Time.deltaTime * staminaRate;
            }
        }

        if (shield)
        {
            if (playerData.equippedEmblems.Contains(emblemLibrary.shell_company))
            {
                LoseMana(Time.deltaTime * blockManaCost / 2);
            }
            else
            {
                LoseMana(Time.deltaTime * blockManaCost);
            }

            if(playerData.mana <= 0)
            {
                playerAnimation.StopBlocking();
            }
        }

        if(manaDelay <= 0)
        {
            if(playerData.mana < playerData.maxMana)
            {
                playerData.mana += Time.deltaTime * manaRechargeRate;
                if (playerData.equippedEmblems.Contains("Magical Acceleration"))
                {
                    playerData.mana += Time.deltaTime * manaRechargeRate;
                }
            }
        }
        else
        {
            manaDelay -= Time.deltaTime;
        }

        if(playerData.healCharges < 0)
        {
            playerData.mana = 0;
        }

        if(poise < maxPoise)
        {
            poise += poiseRate * Time.deltaTime;
        }

        if (isStaggered)
        {
            staggerTimer -= Time.deltaTime;
            if (staggerTimer <= 0)
            {
                EndStagger();
            }
        }

        if (duckHealTimer > 0)
        {
            duckHealTimer -= Time.deltaTime;
            if(duckHealTimer <= 0)
            {
                playerAnimation.EndBodyMagic();
            }
            duckHealCounter += duckHealSpeed * Time.deltaTime;
            if(duckHealCounter >= 1)
            {
                int amount = Mathf.FloorToInt(duckHealCounter);
                PartialHeal(amount);
                duckHealCounter -= amount;
            }
        }
    }

    public void Death()
    {
        playerData.deathNum += 1;
        switch (playerData.deathNum)
        {
            case 1:
                dialogueData.ORTHODOXQueue.Add(1);
                break;
        }

        MaxHeal();
        playerData.healCharges = playerData.maxHealCharges;
        playerData.mana = playerData.maxMana;
        playerData.hasSpawned = false;
        playerData.lostMoney = playerData.money;
        playerData.money = 0;
        mapData.doorNumber = 0;
        mapData.deadEnemies.Clear();
        mapData.usedAltars.Clear();
        mapData.deathPosition = transform.position;
        mapData.deathRoom = SceneManager.GetActiveScene().name;
        gm.SaveGame();
        string sceneName = swordSiteDirectory.GetSceneName(playerData.lastSwordSite);
        SceneManager.LoadScene(sceneName);
    }

    public void Rest()
    {
        MaxHeal();
        playerData.mana = playerData.maxMana;
        playerData.healCharges = playerData.maxHealCharges;
        playerData.hasSpawned = false;
        gm.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
