using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchEffects : MonoBehaviour
{
    //Input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] GameObject pathTrailPrefab;
    [SerializeField] AttackProfiles parryProfile;

    //player scripts
    PlayerEvents playerEvents;
    WeaponManager weaponManager;
    PlayerScript playerScript;
    PlayerHealth playerHealth;
    PlayerAbilities playerAbilities;
    PlayerSound playerSound;
    PlayerAnimation playerAnimation;

    //other scripts
    CameraFollow cameraScript;
    GameManager gm;

    //damage multipliers
    float closeCallDamage = 0.3f;
    float arcaneRemainsDamage = 0.5f;
    float confidentKillerDamage = 0.4f;
    float spellswordDamage = 0.5f;
    float recklessAttackDamage = 0.6f;

    //patch related variables
    [System.NonSerialized] public bool arcaneStepActive = false;
    [System.NonSerialized] public bool arcaneRemainsActive = false;
    float arcaneStepMaxTime = 0.03f;
    float arcaneStepTimer;

    float closeCallMaxTime = 5;
    [System.NonSerialized] public float closeCallTimer;

    [System.NonSerialized] public float mirrorCloakTimer;
    [System.NonSerialized] public float mirrorCloakMaxTime = 5;

    [System.NonSerialized] public bool barrier = false;
    [System.NonSerialized] public float barrierTimer;
    [System.NonSerialized] public float maxBarrierTimer = 10f;

    int explosiveHealingDamage;
    float explosiveHealingRange = 5;
    float explosiveHealingStagger = 1f;

    [System.NonSerialized] public bool deathAuraActive = false;

    float recklessAttackHealthMax = 0.3f;


    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
    }

    private void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
        playerScript = GetComponent<PlayerScript>();
        playerAbilities = GetComponent<PlayerAbilities>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerSound = GetComponentInChildren<PlayerSound>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        gm = GlobalEvents.instance.gameObject.GetComponent<GameManager>();
        barrierTimer = 0;

        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step))
        {
            Physics.IgnoreLayerCollision(8, 6, true);
        }
        else
        {
            Physics.IgnoreLayerCollision(8, 6, false);
        }
    }

    private void Update()
    {
        if (closeCallTimer > 0)
        {
            closeCallTimer -= Time.deltaTime;
            if (closeCallTimer <= 0)
            {
                weaponManager.RemoveWeaponMagicSource();
            }
        }

        if (mirrorCloakTimer > 0)
        {
            mirrorCloakTimer -= Time.deltaTime;
            if (mirrorCloakTimer <= 0) playerEvents.StartMirrorCloak();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step) && arcaneStepActive)
        {
            if (arcaneStepTimer < 0)
            {
                GameObject pathTrail;
                pathTrail = Instantiate(pathTrailPrefab);
                pathTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                arcaneStepTimer = arcaneStepMaxTime;
            }
            else
            {
                arcaneStepTimer -= Time.deltaTime;
            }
        }

        if (barrierTimer > 0)
        {
            barrierTimer -= Time.deltaTime;
        }
        else if (playerData.equippedEmblems.Contains(emblemLibrary.protective_barrier))
        {
            barrier = true;
        }
    }

    public void PerfectDodge(EnemyAttackType enemyAttackType, GameObject projectile = null, EnemyScript attackingEnemy = null)
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary.close_call))
        {
            if (closeCallTimer <= 0)
            {
                weaponManager.AddWeaponMagicSource();
            }
            closeCallTimer = closeCallMaxTime;
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.adrenaline_rush))
        {
            playerScript.stamina = playerData.MaxStamina();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.mirror_cloak) && mirrorCloakTimer <= 0 && attackingEnemy != null)
        {
            playerSound.Shield();
            playerAbilities.BlockOrParry(enemyAttackType, attackingEnemy);
        }
    }

    public int PhysicalDamageModifiers(int physicalDamage)
    {
        float extraDamage = 0;
        if (playerData.equippedEmblems.Contains(emblemLibrary.close_call) && closeCallTimer > 0)
        {
            extraDamage += physicalDamage * closeCallDamage;
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary._spellsword) && playerData.mana > emblemLibrary.spellswordManaCost)
        {
            extraDamage += physicalDamage * spellswordDamage;
            playerScript.LoseMana(emblemLibrary.spellswordManaCost);
        }

        return physicalDamage + Mathf.RoundToInt(extraDamage);
    }

    public int ArcaneDamageModifiers(int arcaneDamage)
    {
        return arcaneDamage;
    }

    public int TotalDamageModifiers(int totalDamage)
    {
        float extraDamage = 0;
        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains) && arcaneRemainsActive)
        {
            extraDamage += totalDamage * arcaneRemainsDamage;
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.confident_killer) && playerData.health == playerData.MaxHealth())
        {
            extraDamage += totalDamage * confidentKillerDamage;
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.reckless_attack) && playerData.health < playerData.MaxHealth() * recklessAttackHealthMax)
        {
            extraDamage += totalDamage * recklessAttackDamage;
        }

        return totalDamage + Mathf.RoundToInt(extraDamage);
    }

    private void onPlayerStagger(object sender, System.EventArgs e)
    {
        EndArcaneStep();
    }

    public void ExplosiveHealing()
    {
        StartCoroutine(cameraScript.ScreenShake(0.1f, 0.3f));
        playerSound.PlaySoundEffectFromList(10, 1);
        playerAnimation.shoveVFX.Play();
        explosiveHealingDamage = playerData.ArcaneDamage() * 3;

        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < explosiveHealingRange)
            {

                enemy.LoseHealth(explosiveHealingDamage, explosiveHealingDamage);
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.StartStagger(explosiveHealingStagger);
            }
        }
    }

    public void StartArcaneStep()
    {
        arcaneStepActive = true;
        arcaneStepTimer = arcaneStepMaxTime;
    }

    public void EndArcaneStep()
    {
        arcaneStepActive = false;
        arcaneStepTimer = 0;
    }

    private void onEnemyKilled(object sender, System.EventArgs e)
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary.vampiric_strikes))
        {
            int healAmount = Mathf.FloorToInt(playerData.MaxHealth() / 5);
            playerHealth.PartialHeal(healAmount);
        }
    }

    private void OnEnable()
    {
        playerEvents.onPlayerStagger += onPlayerStagger;
        GlobalEvents.instance.onEnemyKilled += onEnemyKilled;
    }


    private void OnDisable()
    {
        playerEvents.onPlayerStagger -= onPlayerStagger;
        GlobalEvents.instance.onEnemyKilled -= onEnemyKilled;
    }
}
