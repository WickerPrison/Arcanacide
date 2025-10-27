using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchEffects : MonoBehaviour, IDamageEnemy
{
    //Input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] GameObject arcaneStepHolderPrefab;
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
    Vector3 previousPosition;
    float dist;

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

    public bool blockable { get; set; }

    [System.NonSerialized] public List<PathTrail> pathTrails = new List<PathTrail>();

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

        ArcaneStepDodgeThrough();
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

        if (barrierTimer > 0)
        {
            barrierTimer -= Time.deltaTime;
        }
        else if (playerData.equippedPatches.Contains(Patches.PROTECTIVE_BARRIER))
        {
            barrier = true;
        }
    }

    private void FixedUpdate()
    {
        if (playerData.equippedPatches.Contains(Patches.ARCANE_STEP) && arcaneStepActive)
        {
            dist += Vector3.Distance(transform.position, previousPosition);
            if(dist > 0.7f)
            {
                SpawnArcaneStep();
            }
            previousPosition = transform.position;
        }
    }

    public void PerfectDodge(EnemyAttackType enemyAttackType, GameObject projectile = null, EnemyScript attackingEnemy = null)
    {
        if (playerData.equippedPatches.Contains(Patches.CLOSE_CALL))
        {
            if (closeCallTimer <= 0)
            {
                weaponManager.AddWeaponMagicSource();
            }
            closeCallTimer = closeCallMaxTime;
        }

        if (playerData.equippedPatches.Contains(Patches.ADRENALINE_RUSH))
        {
            playerScript.GainStamina(playerData.MaxStamina());
        }

        if (playerData.equippedPatches.Contains(Patches.MIRROR_CLOAK) && mirrorCloakTimer <= 0 && attackingEnemy != null)
        {
            playerSound.PlaySoundEffect(PlayerSFX.SHIELD, 1);
            playerAbilities.BlockOrParry(enemyAttackType, attackingEnemy);
        }
    }

    public int PhysicalDamageModifiers(int physicalDamage)
    {
        float extraDamage = 0;
        if (playerData.equippedPatches.Contains(Patches.CLOSE_CALL) && closeCallTimer > 0)
        {
            extraDamage += physicalDamage * closeCallDamage;
        }

        if (playerData.equippedPatches.Contains(Patches.SPELLSWORD) && playerData.mana > emblemLibrary.spellsword.value)
        {
            extraDamage += physicalDamage * spellswordDamage;
            playerScript.LoseMana(emblemLibrary.spellsword.value);
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
        if (playerData.equippedPatches.Contains(Patches.ARCANE_REMAINS) && arcaneRemainsActive)
        {
            extraDamage += totalDamage * arcaneRemainsDamage;
        }

        if (playerData.equippedPatches.Contains(Patches.CONFIDENT_KILLER) && playerData.health == playerData.MaxHealth())
        {
            extraDamage += totalDamage * confidentKillerDamage;
        }

        if (playerData.equippedPatches.Contains(Patches.RECKLESS_ATTACK) && playerData.health < playerData.MaxHealth() * recklessAttackHealthMax)
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
        playerSound.PlaySoundEffect(PlayerSFX.BIGSMASH, 1);
        playerAnimation.shoveVFX.Play();
        explosiveHealingDamage = playerData.ArcaneDamage() * 3;

        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < explosiveHealingRange)
            {
                blockable = true;
                enemy.LoseHealth(explosiveHealingDamage, explosiveHealingDamage, this, () =>
                {
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    enemyController.StartStagger(explosiveHealingStagger);
                });
            }
        }
    }

    public void StartArcaneStep()
    {
        arcaneStepActive = true;
        previousPosition = transform.position;
        SpawnArcaneStep();
    }

    public void EndArcaneStep()
    {
        arcaneStepActive = false;
    }

    void SpawnArcaneStep()
    {
        dist = 0;
        GameObject pathTrail;
        pathTrail = Instantiate(pathTrailPrefab);
        pathTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        pathTrail.GetComponent<PathTrail>().pathTrails = pathTrails;
    }

    public void ArcaneStepDodgeThrough()
    {
        if (playerData.equippedPatches.Contains(Patches.ARCANE_STEP))
        {
            Physics.IgnoreLayerCollision(8, 6, true);
        }
        else
        {
            Physics.IgnoreLayerCollision(8, 6, false);
        }
    }

    private void onEnemyKilled(object sender, System.EventArgs e)
    {
        if (playerData.equippedPatches.Contains(Patches.VAMPIRIC_STRIKES))
        {
            int healAmount = Mathf.FloorToInt(playerData.MaxHealth() * emblemLibrary.patchDictionary[Patches.VAMPIRIC_STRIKES].value);
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
