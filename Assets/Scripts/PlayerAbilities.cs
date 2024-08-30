using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    //Input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] AttackProfiles parryProfile;
    [SerializeField] List<AttackProfiles> specialAttackProfiles;
    [SerializeField] AttackProfiles axeHeavyProfile;
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] PlayerProjectile projectilePrefab;
    [SerializeField] Bolts bolts;
    [SerializeField] Transform[] boltsOrigin;
    [SerializeField] ExternalLanternFairy lanternFairy;
    [SerializeField] Transform[] internalLanternFairies;
    [SerializeField] GameObject fairyProjectilePrefab;
    [SerializeField] Transform frontSwordTip;
    [SerializeField] Transform backSwordTip;
    [SerializeField] GameObject totemPrefab;
    [SerializeField] Transform attackPoint;

    //player scripts
    PlayerMovement playerMovement;
    PlayerScript playerScript;
    PlayerAnimation playerAnimation;
    PlayerEvents playerEvents;
    PatchEffects patchEffects;
    WeaponManager weaponManager;
    PlayerSound playerSound;
    Rigidbody rb;

    //managers
    GameManager gm;
    InputManager im;

    //other variables
    WaitForSeconds parryWindow = new WaitForSeconds(0.2f);
    [System.NonSerialized] public bool parry;
    [System.NonSerialized] public bool shield;
    float parryCost = 20;
    int blockManaCost = 15;

    float shoveRadius = 3;
    float shovePoiseDamage = 100;

    bool heavyAttackActive = false;

    bool backstepActive = false;
    float backstepTimer;
    float backstepMaxTime = 0.015f;

    bool knifeSpecialAttackOn = false;
    Vector3 away = Vector3.one * 100;
    float boltdamage = 0;

    float clawSpecialMaxTime = 15f;
    float clawSpecialDamageMult = 2;
    [System.NonSerialized] public float clawSpecialStamCostMult = 2;
    [System.NonSerialized] public float clawSpecialTakeDamageMult = 0.5f;

    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
        bolts.SetPositions(away, away);
    }

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
        playerScript = GetComponent<PlayerScript>();
        patchEffects = GetComponent<PatchEffects>();
        weaponManager = GetComponent<WeaponManager>();
        playerSound = GetComponentInChildren<PlayerSound>();;
        rb = GetComponent<Rigidbody>();

        if (playerData.swordSpecialTimer > 0) weaponManager.AddSpecificWeaponSource(0);

        SetupControls();
    }

    private void Update()
    {
        if(playerData.swordSpecialTimer > 0)
        {
            playerData.swordSpecialTimer -= Time.deltaTime;
            if(playerData.swordSpecialTimer <= 0)
            {
                weaponManager.RemoveSpecificWeaponSource(0);
            }    
        }

        if (knifeSpecialAttackOn)
        {
            UpdateKnifeSpecialAttack();
        }

        if (shield)
        {
            if (playerData.equippedEmblems.Contains(emblemLibrary.shell_company))
            {
                playerScript.LoseMana(Time.deltaTime * blockManaCost / 2);
            }
            else
            {
                playerScript.LoseMana(Time.deltaTime * blockManaCost);
            }

            if (playerData.mana <= 0)
            {
                playerAnimation.PlayAnimation("StopBlocking");
            }
        }

        if (playerData.currentWeapon == 1 && backstepActive)
        {
            if (backstepTimer <= 0)
            {
                GameObject pathTrail;
                pathTrail = Instantiate(fireTrailPrefab);
                pathTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                backstepTimer = backstepMaxTime;
            }
            else
            {
                backstepTimer -= Time.deltaTime;
            }
        }
    }

    public int DetermineAttackDamage(AttackProfiles attackProfile, float chargeDecimal = 0)
    {
        int physicalDamage = Mathf.RoundToInt(playerData.PhysicalDamage() * attackProfile.damageMultiplier);
        if (chargeDecimal >= 1)
        {
            physicalDamage += Mathf.RoundToInt(playerData.PhysicalDamage() * attackProfile.chargeDamage * (chargeDecimal + attackProfile.fullChargeDamage));
        }
        else if(chargeDecimal > 0) 
        {
            physicalDamage += Mathf.RoundToInt(playerData.PhysicalDamage() * attackProfile.chargeDamage * chargeDecimal);
        }
        physicalDamage = patchEffects.PhysicalDamageModifiers(physicalDamage);

        int arcaneDamage = Mathf.RoundToInt(playerData.ArcaneDamage() * attackProfile.magicDamageMultiplier);
        arcaneDamage = patchEffects.ArcaneDamageModifiers(arcaneDamage);

        int totalDamage = physicalDamage + arcaneDamage;
        totalDamage = patchEffects.TotalDamageModifiers(totalDamage);
        totalDamage = DamageModifiers(totalDamage);
        return totalDamage; 
    }

    public void DamageEnemy(EnemyScript enemy, int damage, AttackProfiles attackProfile)
    {
        if (!attackProfile.soundOnHitEvent.IsNull)
        {
            playerSound.PlaySoundEffect(attackProfile.soundOnHitEvent, attackProfile.soundOnHitVolume);
        }

        if (enemy.DOT > 0 && playerData.equippedEmblems.Contains(emblemLibrary.opportune_strike))
        {
            damage = Mathf.RoundToInt(damage * 1.2f);
        }

        enemy.LoseHealth(damage, damage * attackProfile.poiseDamageMultiplier);
        enemy.ImpactVFX();
        if (attackProfile.attackType == AttackType.HEAVY && playerData.equippedEmblems.Contains(emblemLibrary.rending_blows))
        {
            enemy.GainDOT(emblemLibrary.rendingBlowsDuration);
        }

        enemy.GainDOT(attackProfile.durationDOT);

        if (attackProfile.staggerDuration > 0)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.StartStagger(attackProfile.staggerDuration);
        }
    }

    public int DamageModifiers(int attackPower)
    {
        float extraDamage = 0;
        if(playerData.swordSpecialTimer > 0 && playerData.currentWeapon == 0)
        {
            extraDamage += attackPower * specialAttackProfiles[0].damageMultiplier;

            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_mastery))
            {
                extraDamage += attackPower * emblemLibrary.arcaneMasteryPercent;
            }
        }

        if (playerData.clawSpecialOn)
        {
            extraDamage += attackPower * clawSpecialDamageMult;

            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_mastery))
            {
                extraDamage += attackPower * emblemLibrary.arcaneMasteryPercent;
            }
        }

        return attackPower + Mathf.RoundToInt(extraDamage);
    }

    public void Shield()
    {
        if (!playerData.unlockedAbilities.Contains("Block") || !playerMovement.CanInput()) return;
        
        if (playerData.mana > 0)
        {
            rb.velocity = Vector3.zero;
            playerAnimation.PlayAnimation("Block");
            playerAnimation.continueBlocking = true;
        }
    }

    public void BlockOrParry(EnemyAttackType attackType, EnemyScript attackingEnemy)
    {
        switch (attackType)
        {
            case EnemyAttackType.PROJECTILE:
                playerSound.PlaySoundEffect(PlayerSFX.RING, 0.5f);
                FireProjectile(attackingEnemy, new Vector3(transform.position.x, 1.1f, transform.position.z), parryProfile);
                break;
            case EnemyAttackType.MELEE:
                playerEvents.MeleeParry();
                playerSound.PlaySoundEffect(PlayerSFX.RING, 0.5f);
                attackingEnemy.LosePoise((playerData.ArcaneDamage() + playerData.PhysicalDamage()) * parryProfile.poiseDamageMultiplier);
                attackingEnemy.ImpactVFX();
                break;
        }
    }

    public void Shove()
    {
        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= shoveRadius)
            {
                enemy.LosePoise(shovePoiseDamage);
                EnemyController enemyController = enemy.gameObject.GetComponent<EnemyController>();
                enemyController.StartStagger(0.5f);
            }
        }
    }

    void Attack()
    {
        if (shield && !parry)
        {
            StartCoroutine(Parry());
            return;
        }

        if (playerAnimation.attacking)
        {
            playerAnimation.ChainAttacks();
        }
        else if (playerMovement.CanInput() && playerScript.stamina > 0)
        {
            rb.velocity = Vector3.zero;
            playerAnimation.attacking = true;
            playerAnimation.PlayAnimation("Attack");
        }
    }

    IEnumerator Parry()
    {
        playerScript.LoseMana(parryCost);
        parry = true;
        yield return parryWindow;
        parry = false;
    }

    void HeavyAttack()
    {
        if (playerAnimation.attacking)
        {
            playerAnimation.Combo();
        }
        else if (playerMovement.CanInput() && playerScript.stamina > 0)
        {
            if (playerData.currentWeapon == 1 && !lanternFairy.isInLantern) return;
            heavyAttackActive = true;
            rb.velocity = Vector3.zero;
            playerAnimation.attacking = playerData.currentWeapon != 3;
            if (playerData.currentWeapon == 0) playerAnimation.SetBool("chargeHeavy", true);
            playerAnimation.PlayAnimation("HeavyAttack");
        }
    }

    void EndHeavyAttack()
    {
        if (playerData.currentWeapon == 3 && heavyAttackActive)
        {
            playerAnimation.PlayAnimation("EndHeavyAttack");
        }
        else if(playerData.currentWeapon == 0)
        {
            playerAnimation.SetBool("chargeHeavy", false);
        }

        heavyAttackActive = false;
    }

    public void AxeHeavy()
    {
        playerScript.LoseStamina(axeHeavyProfile.staminaCost);
        FairyProjectile fairyProjectile = Instantiate(fairyProjectilePrefab).GetComponent<FairyProjectile>();
        if (playerAnimation.facingFront)
        {
            fairyProjectile.transform.position = internalLanternFairies[0].transform.position;
        }
        else
        {
            fairyProjectile.transform.position = internalLanternFairies[1].transform.position;
        }
        fairyProjectile.direction = attackPoint.position - transform.position;
        fairyProjectile.lanternFairy = lanternFairy;
        fairyProjectile.playerAbilities = this;
    }

    public void SpecialAttack()
    {
        if (!playerData.unlockedAbilities.Contains("Special Attack")) return;

        if (playerMovement.CanInput() && playerScript.stamina > 0 && playerData.mana >= specialAttackProfiles[playerData.currentWeapon].manaCost)
        {
            if (playerData.currentWeapon == 1)
            {
                AxeSpecialAttack();
            }
            else
            {
                rb.velocity = Vector3.zero;
                playerAnimation.attacking = true;
                playerAnimation.PlayAnimation("SpecialAttack");
            }
        }
    }

    void EndSpecialAttack()
    {
        if (playerData.currentWeapon == 2)
        {
            knifeSpecialAttackOn = false;
            bolts.SetPositions(away, away);
            bolts.SoundOff();
            playerAnimation.PlayAnimation("EndSpecialAttack");
        }
    }

    public void SwordSpecialAttack()
    {
        playerScript.LoseStamina(specialAttackProfiles[0].staminaCost);
        playerScript.LoseMana(specialAttackProfiles[0].manaCost);

        playerData.swordSpecialTimer = specialAttackProfiles[0].bonusEffectDuration;
        weaponManager.AddSpecificWeaponSource(0);
    }

    public void FireProjectile(EnemyScript enemy, Vector3 startingPosition, AttackProfiles attackProfile)
    {
        PlayerProjectile projectile = Instantiate(projectilePrefab).GetComponent<PlayerProjectile>();
        projectile.attackProfile = attackProfile;
        projectile.transform.position = startingPosition;
        projectile.transform.LookAt(enemy.transform.position + new Vector3(0, 1.1f, 0));
        projectile.target = enemy.transform;
        projectile.playerController = playerMovement;
    }

    public void AxeSpecialAttack()
    {
        if (!lanternFairy.isInLantern) return;

        playerScript.LoseMana(specialAttackProfiles[1].manaCost);
        TotemAnimationEvents totemAnimEvents = Instantiate(totemPrefab).GetComponentInChildren<TotemAnimationEvents>();
        totemAnimEvents.transform.parent.position = new Vector3(transform.position.x, 0, transform.position.z);
        totemAnimEvents.lanternFairy = lanternFairy;
        playerEvents.AxeSpecialAttack();
    }

    public void KnifeSpecialAttack()
    {
        knifeSpecialAttackOn = true;
    }

    void UpdateKnifeSpecialAttack()
    {
        playerData.mana -= Time.deltaTime * specialAttackProfiles[2].manaCost;
        if (playerData.mana <= 0)
        {
            EndSpecialAttack();
            return;
        }

        EnemyScript closestEnemy = null;
        float distance = 10;
        foreach (EnemyScript enemy in gm.enemies)
        {
            float enemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
            if (enemyDistance < distance)
            {
                distance = enemyDistance;
                closestEnemy = enemy;
            }
        }

        int boltsFrontOrBack = 0;
        if (playerAnimation.facingDirection > 1)
        {
            boltsFrontOrBack = 1;
        }

        if (closestEnemy != null)
        {
            bolts.SetPositions(boltsOrigin[boltsFrontOrBack].position, closestEnemy.transform.position + new Vector3(0, 1.1f, 0));
            bolts.SoundOn();
            boltdamage += playerData.arcane * specialAttackProfiles[2].magicDamageMultiplier * Time.deltaTime;
            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_mastery))
            {
                boltdamage += boltdamage * emblemLibrary.arcaneMasteryPercent;
            }

            if (boltdamage > 1)
            {
                closestEnemy.LoseHealth(Mathf.FloorToInt(boltdamage), 0);
                boltdamage = 0;
            }
        }
        else
        {
            bolts.SetPositions(away, away);
            bolts.SoundOff();
        }
    }

    private void onClawSpecial(object sender, System.EventArgs e)
    {
        playerScript.LoseMana(specialAttackProfiles[3].manaCost);
        playerData.clawSpecialTimer = clawSpecialMaxTime;
        playerData.clawSpecialOn = true;
    }


    private void onPlayerStagger(object sender, System.EventArgs e)
    {
        if (playerData.currentWeapon == 2 && knifeSpecialAttackOn)
        {
            knifeSpecialAttackOn = false;
            bolts.SetPositions(away, away);
        }
    }

    private void onBackstepStart(object sender, System.EventArgs e)
    {
        backstepActive = true;
    }

    private void PlayerEvents_onDashEnd(object sender, System.EventArgs e)
    {
        backstepActive = false;
    }

    void SetupControls()
    {
        im = gm.GetComponent<InputManager>();

        im.controls.Gameplay.Attack.performed += ctx => Attack();
        im.controls.Gameplay.HeavyAttack.performed += ctx => HeavyAttack();
        im.controls.Gameplay.HeavyAttack.canceled += ctx => EndHeavyAttack();
        im.controls.Gameplay.SpecialAttack.performed += ctx => SpecialAttack();
        im.controls.Gameplay.SpecialAttack.canceled += ctx => EndSpecialAttack();
        im.controls.Gameplay.Shield.performed += ctx => Shield();
        im.controls.Gameplay.Shield.canceled += ctx => playerAnimation.continueBlocking = false;
        im.controls.Gameplay.Heal.performed += ctx => playerAnimation.HealAnimation();
    }

    private void OnEnable()
    {
        playerEvents.onPlayerStagger += onPlayerStagger;
        playerEvents.onClawSpecial += onClawSpecial;
        playerEvents.onBackstepStart += onBackstepStart;
        playerEvents.onDashEnd += PlayerEvents_onDashEnd;
    }

    private void OnDisable()
    {
        playerEvents.onPlayerStagger -= onPlayerStagger;
        playerEvents.onClawSpecial -= onClawSpecial;
        playerEvents.onBackstepStart -= onBackstepStart;
        playerEvents.onDashEnd -= PlayerEvents_onDashEnd;
    }
}
