using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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

    //player scripts
    PlayerAbilities playerAbilities;
    PlayerMovement playerMovement;
    PatchEffects patchEffects;
    PlayerAnimation playerAnimation;
    PlayerEvents playerEvents;
    PlayerHealth playerHealth;
    PlayerSound playerSound;

    //other scripts
    GameManager gm;

    //stamina variables
    public float stamina { get; private set; }
    float maxStaminaDelay = 1f;
    float staminaDelay;
    float staminaRate = 40;

    //stagger variables
    [System.NonSerialized] public bool isStaggered = false;
    float staggerTimer = 0;

    //poise variables
    [System.NonSerialized] public float poise;
    float maxPoise = 100;
    float poiseRate = 5;
    float poiseBreakStagger = 1;

    //mana variables
    float manaDelay;
    [System.NonSerialized] public float maxManaDelay = 2;
    [System.NonSerialized] public float manaRechargeRate = 4;
    float _magicalAccelerationValue;
    float magicalAccelerationValue
    {
        get
        {
            if(_magicalAccelerationValue == 0)
            {
                _magicalAccelerationValue = emblemLibrary.patchDictionary[Patches.MAGICAL_ACCELERATION].value;
            }
            return _magicalAccelerationValue;
        }
    }

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
        playerMovement = GetComponent<PlayerMovement>();
        patchEffects = GetComponent<PatchEffects>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerHealth = GetComponent<PlayerHealth>();
        playerSound = GetComponentInChildren<PlayerSound>();

        GlobalEvents.instance.onTestButton += Instance_onTestButton;
    }

    private void Instance_onTestButton(object sender, System.EventArgs e)
    {
        if (Time.timeScale == 1) Time.timeScale = 0;
        else Time.timeScale = 1;
        //LoseHealth(25, EnemyAttackType.NONPARRIABLE, null);
    }

    public void HitPlayer(Action onHit, Action onDodge)
    {
        if(gameObject.layer == 3)
        {
            onHit();
        }
        else
        {
            onDodge();
        }
    }

    public void LoseHealth(int damage, EnemyAttackType attackType, EnemyScript attackingEnemy)
    {
        if (!playerAbilities.shield)
        {
            playerHealth.LoseHealth(damage, attackType, attackingEnemy);
        }
        else
        {
            playerSound.PlaySoundEffect(PlayerSFX.SHIELD, 1);
            if (!playerAbilities.parry || attackingEnemy == null) return;
            playerAbilities.BlockOrParry(attackType, attackingEnemy);
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
        if(staggerDuration > staggerTimer && !playerAbilities.shield)
        {
            playerMovement.isDashing = false;

            staggerTimer = staggerDuration;
            
            isStaggered = true;

            playerEvents.PlayerStagger();
        }
    }

    public void EndStagger()
    {
        staggerTimer = 0;
        isStaggered = false;
        playerAnimation.PlayAnimation("Idle");
        playerEvents.EndPlayerStagger();
    }

    public void LoseStamina(float amount)
    {
        if (playerData.clawSpecialOn) amount *= playerAbilities.clawSpecialStamCostMult;

        stamina -= amount;
        if(stamina < 0)
        {
            stamina = 0;
        }
        GlobalEvents.instance.LoseStamina(stamina);
        playerAnimation.StaminaUpdate();
        staminaDelay = maxStaminaDelay;
    }

    public void GainStamina(float amount)
    {
        stamina += amount;
        if(stamina > playerData.MaxStamina())
        {
            stamina = playerData.MaxStamina();
        }

        GlobalEvents.instance.GainStamina(stamina);
    }

    public void LoseMana(float amount)
    {
        playerData.mana -= amount;
        if(playerData.mana < 0)
        {
            playerData.mana = 0;
        }
        manaDelay = maxManaDelay;
        if (playerData.equippedPatches.Contains(Patches.MAGICAL_ACCELERATION))
        {
            manaDelay = maxManaDelay / magicalAccelerationValue;
        }
        if (patchEffects.deathAuraActive)
        {
            manaDelay = maxManaDelay / 2;
        }
    }

    public void PerfectDodge(EnemyAttackType attackType, EnemyScript attackingEnemy = null, GameObject projectile = null)
    {
        patchEffects.PerfectDodge(attackType, projectile, attackingEnemy);
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
                GainStamina(Time.deltaTime * staminaRate);
            }
        }

        if(manaDelay <= 0)
        {
            if(playerData.mana < playerData.maxMana)
            {
                float rechargeRateMod = 1;
                if (playerData.equippedPatches.Contains(Patches.MAGICAL_ACCELERATION))
                {
                    rechargeRateMod *= magicalAccelerationValue;
                }
                if (patchEffects.deathAuraActive)
                {
                    rechargeRateMod *= 2;
                }
                playerData.mana += Time.deltaTime * manaRechargeRate * rechargeRateMod;
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
                dialogueData.directorQueue.Add(1);
                break;
        }

        playerHealth.MaxHeal();
        playerData.healCharges = playerData.maxHealCharges;
        playerData.mana = playerData.maxMana;
        playerData.hasSpawned = false;
        if (playerData.equippedPatches.Contains(Patches.STANDARD_DEDUCTION))
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
        mapData.deathPosition = transform.position;
        gm.SaveGame();
        MusicPlayer musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        musicPlayer.currentTrack = Music.NONE;
        string sceneName = gm.GetSceneName(playerData.lastSwordSite);
        SceneManager.LoadScene(sceneName);
    }

    public void Rest()
    {
        playerHealth.MaxHeal();
        playerData.mana = playerData.maxMana;
        playerData.healCharges = playerData.maxHealCharges;
        if (playerData.equippedPatches.Contains(Patches.MAXIMUM_REFUND))
        {
            playerData.healCharges += 1;
        }
        playerData.hasSpawned = false;
        gm.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnPlayerMoneyChange(GlobalEvents sender, int amount)
    {
        playerData.money += amount;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onPlayerMoneyChange += OnPlayerMoneyChange;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onPlayerMoneyChange -= OnPlayerMoneyChange;
    }
}
