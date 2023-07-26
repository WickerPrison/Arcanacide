using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnemyAttackType
{
    NONPARRIABLE, PROJECTILE, MELEE
}

public class PlayerScript : MonoBehaviour
{
    //This Script manages stamina, mana, as well as the player state
    //This should normally be the only player script an enemy has to reference

    //input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] AttackProfiles parryProfile;

    //player scripts
    PlayerAbilities playerAbilities;
    PatchEffects patchEffects;
    PlayerAnimation playerAnimation;
    PlayerEvents playerEvents;
    PlayerSound playerSound;
    PlayerHealth playerHealth;
    PlayerSound sfx;

    //other scripts
    GameManager gm;

    //stagger variables
    [System.NonSerialized] public float stamina;
    [System.NonSerialized] public bool isStaggered = false;
    float maxStaminaDelay = 1f;
    float staminaDelay;
    float staminaRate = 40;
    float staggerTimer = 0;

    //poise variables
    [System.NonSerialized] public float poise;
    float maxPoise = 100;
    float poiseRate = 5;
    float poiseBreakStagger = 1;

    //mana variables
    float manaDelay;
    float maxManaDelay = 2;
    float manaRechargeRate = 4;

    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stamina = playerData.MaxStamina();
        poise = maxPoise;
        playerAbilities = GetComponent<PlayerAbilities>();
        patchEffects = GetComponent<PatchEffects>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerSound = GetComponentInChildren<PlayerSound>();
        playerHealth = GetComponent<PlayerHealth>();
        sfx = GetComponentInChildren<PlayerSound>();

        GlobalEvents.instance.onTestButton += Instance_onTestButton;
    }

    private void Instance_onTestButton(object sender, System.EventArgs e)
    {
        StartStagger(1.1f);
    }

    public void LoseHealth(int damage, EnemyAttackType attackType, EnemyScript attackingEnemy)
    {
        if (!playerAbilities.shield)
        {
            playerHealth.LoseHealth(damage, attackType, attackingEnemy);
        }
        else
        {
            // eventually move this to playerAbilities and connect with playerEvents
            sfx.Shield();
            if (!playerAbilities.parry || attackingEnemy == null) return;
            switch (attackType)
            {
                case EnemyAttackType.PROJECTILE:
                    playerSound.PlaySoundEffectFromList(11, 0.5f);
                    playerAbilities.FireProjectile(attackingEnemy, new Vector3(transform.position.x, 1.1f, transform.position.z), parryProfile);
                    break;
                case EnemyAttackType.MELEE:
                    playerEvents.MeleeParry();
                    playerSound.PlaySoundEffectFromList(11, 0.5f);
                    attackingEnemy.LosePoise((playerData.ArcaneDamage() + playerData.PhysicalDamage()) * parryProfile.poiseDamageMultiplier);
                    attackingEnemy.ImpactVFX();
                    break;
            }
        }
    }

    public void LosePoise(float poiseDamage)
    {
        if (!playerAbilities.shield)
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
        if(staggerDuration > 0 && !playerAbilities.shield)
        {
            staggerTimer += staggerDuration;
            isStaggered = true;

            playerEvents.PlayerStagger();
        }
    }

    public void EndStagger()
    {
        staggerTimer = 0;
        isStaggered = false;
        playerAnimation.PlayAnimation("Idle");
    }

    public void LoseStamina(float amount)
    {
        if (playerData.clawSpecialOn) amount *= playerAbilities.clawSpecialStamCostMult;

        stamina -= amount;
        if(stamina < 0)
        {
            stamina = 0;
        }
        playerAnimation.StaminaUpdate();
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
        if (patchEffects.deathAuraActive)
        {
            manaDelay = maxManaDelay / 2;
        }
    }

    public void PerfectDodge(GameObject projectile = null, EnemyScript attackingEnemy = null)
    {
        patchEffects.PerfectDodge(projectile, attackingEnemy);
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

        if(manaDelay <= 0)
        {
            if(playerData.mana < playerData.maxMana)
            {
                playerData.mana += Time.deltaTime * manaRechargeRate;
                if (playerData.equippedEmblems.Contains("Magical Acceleration"))
                {
                    playerData.mana += Time.deltaTime * manaRechargeRate;
                }
                if (patchEffects.deathAuraActive)
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

            if(playerData.currentWeapon == 1)
            {
                staggerTimer -= Time.deltaTime;
            }

            if (staggerTimer <= 0)
            {
                EndStagger();
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

        playerHealth.MaxHeal();
        playerData.healCharges = playerData.maxHealCharges;
        playerData.mana = playerData.maxMana;
        playerData.hasSpawned = false;
        if (playerData.equippedEmblems.Contains(emblemLibrary.charons_obol))
        {
            playerData.lostMoney = 0;
            playerData.money = Mathf.RoundToInt(playerData.money / 2);
            mapData.deathRoom = "none";
        }
        else
        {
            playerData.lostMoney = playerData.money;
            playerData.money = 0;
            mapData.deathRoom = SceneManager.GetActiveScene().name;
        }
        mapData.doorNumber = 0;
        mapData.deadEnemies.Clear();
        mapData.usedAltars.Clear();
        mapData.deathPosition = transform.position;
        gm.SaveGame();
        string sceneName = gm.GetSceneName(playerData.lastSwordSite);
        SceneManager.LoadScene(sceneName);
    }

    public void Rest()
    {
        playerHealth.MaxHeal();
        playerData.mana = playerData.maxMana;
        playerData.healCharges = playerData.maxHealCharges;
        if (playerData.equippedEmblems.Contains(emblemLibrary.durable_gem))
        {
            playerData.healCharges += 1;
        }
        playerData.hasSpawned = false;
        gm.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
