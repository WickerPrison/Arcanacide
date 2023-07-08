using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] ParticleSystem shoveVFX;
    [SerializeField] List<AttackProfiles> specialAttackProfiles;
    [SerializeField] PlayerProjectile projectilePrefab;
    [SerializeField] Bolts bolts;
    [SerializeField] Transform[] boltsOrigin;
    [SerializeField] ExternalLanternFairy lanternFairy;
    [SerializeField] Transform frontSwordTip;
    [SerializeField] Transform backSwordTip;
    [SerializeField] GameObject totemPrefab;

    PlayerMovement playerController;
    PlayerScript playerScript;
    PlayerAnimation playerAnimation;
    PlayerEvents playerEvents;
    WeaponManager weaponManager;
    PlayerSound playerSound;
    Rigidbody rb;

    GameManager gm;
    InputManager im;

    float shoveManaCost = 20;
    float shoveRadius = 3;
    float shovePoiseDamage = 100;

    WaitForSeconds parryWindow = new WaitForSeconds(0.2f);
    float parryCost = 20;

    bool heavyAttackActive = false;
    float clawSpecialMaxTime = 15f;
    float clawSpecialDamageMult = 2;

    bool knifeSpecialAttackOn = false;

    float boltdamage = 0;

    Vector3 away = Vector3.one * 100;

    [System.NonSerialized] public float axeHeavyTimer = 0;
    [System.NonSerialized] public float axeHeavyMaxTime = 15;

    [System.NonSerialized] public bool arcaneStepActive = false;
    [System.NonSerialized] public bool arcaneRemainsActive = false;
    float arcaneStepMaxTime = 0.03f;
    float arcaneStepTimer;

    float closeCallMaxTime = 5;
    [System.NonSerialized] public float closeCallTimer;
    [SerializeField] GameObject pathTrailPrefab;
    [System.NonSerialized] public float mirrorCloakTimer;
    [System.NonSerialized] public float mirrorCloakMaxTime = 5;

    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
        bolts.SetPositions(away, away);
    }

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerController = GetComponent<PlayerMovement>();
        playerScript = GetComponent<PlayerScript>();
        weaponManager = GetComponent<WeaponManager>();
        playerSound = GetComponentInChildren<PlayerSound>();
        rb = GetComponent<Rigidbody>();

        SetupControls();
    }

    private void Update()
    {
        if (knifeSpecialAttackOn)
        {
            UpdateKnifeSpecialAttack();
        }

        if (axeHeavyTimer > 0)
        {
            axeHeavyTimer -= Time.deltaTime;
            if (axeHeavyTimer <= 0)
            {
                weaponManager.RemoveSpecificWeaponSource(1);
            }
        }

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
    }

    public int DamageModifiers(int attackPower)
    {
        if (playerData.currentWeapon == 1 && axeHeavyTimer > 0)
        {
            attackPower += playerData.ArcaneDamage();
        }

        if (playerData.clawSpecialOn)
        {
            float damageMult;
            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_mastery))
            {
                damageMult = clawSpecialDamageMult + clawSpecialDamageMult * emblemLibrary.arcaneMasteryPercent;
            }
            else damageMult = clawSpecialDamageMult;

            attackPower = Mathf.RoundToInt(attackPower * damageMult);
        }

        return attackPower;
    }

    public void PerfectDog(GameObject projectile = null, EnemyScript attackingEnemy = null)
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

            FireProjectile(attackingEnemy, new Vector3(transform.position.x, 1.1f, transform.position.z), playerScript.parryProfile);
        }
    }

    public void Shield()
    {
        if (!playerData.unlockedAbilities.Contains("Block") || !playerController.CanInput())
        {
            return;
        }

        if (playerData.mana > 0)
        {
            rb.velocity = Vector3.zero;
            playerAnimation.PlayAnimation("Block");
            playerAnimation.continueBlocking = true;
        }
    }

    void Shove()
    {
        if (!playerData.unlockedAbilities.Contains("Shove") || !playerController.CanInput())
        {
            return;
        }

        if (playerData.mana > shoveManaCost)
        {
            playerScript.LoseMana(shoveManaCost);
            rb.velocity = Vector3.zero;
            playerAnimation.PlayAnimation("Shove");
        }
    }


    public void ShoveEffect()
    {
        shoveVFX.Play();
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
        if (playerScript.shield && !playerScript.parry)
        {
            StartCoroutine(Parry());
            return;
        }

        if (playerAnimation.attacking)
        {
            playerAnimation.ChainAttacks();
        }
        else if (playerController.CanInput() && playerScript.stamina > 0)
        {
            rb.velocity = Vector3.zero;
            playerAnimation.attacking = true;
            playerAnimation.PlayAnimation("Attack");
        }
    }

    IEnumerator Parry()
    {
        playerScript.LoseMana(parryCost);
        playerScript.parry = true;
        yield return parryWindow;
        playerScript.parry = false;
    }

    void HeavyAttack()
    {
        if (playerAnimation.attacking)
        {
            playerAnimation.Combo();
        }
        else if (playerController.CanInput() && playerScript.stamina > 0)
        {
            heavyAttackActive = true;
            rb.velocity = Vector3.zero;
            playerAnimation.attacking = playerData.currentWeapon != 3;
            playerAnimation.PlayAnimation("HeavyAttack");
        }
    }

    void EndHeavyAttack()
    {
        if (playerData.currentWeapon == 3 && heavyAttackActive)
        {
            playerAnimation.PlayAnimation("EndHeavyAttack");
        }

        heavyAttackActive = false;
    }

    public void SpecialAttack()
    {
        if (!playerData.unlockedAbilities.Contains("Special Attack")) return;

        if (playerController.CanInput() && playerScript.stamina > 0 && playerData.mana > specialAttackProfiles[playerData.currentWeapon].manaCost)
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
        Transform origin;
        if (playerAnimation.facingFront)
        {
            origin = frontSwordTip;
        }
        else
        {
            origin = backSwordTip;
        }

        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= specialAttackProfiles[0].attackRange)
            {
                FireProjectile(enemy, origin.position, specialAttackProfiles[0]);
            }
        }
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
            boltdamage += playerData.dedication * specialAttackProfiles[2].magicDamageMultiplier * Time.deltaTime;
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

    public void FireProjectile(EnemyScript enemy, Vector3 startingPosition, AttackProfiles attackProfile)
    {
        PlayerProjectile projectile = Instantiate(projectilePrefab).GetComponent<PlayerProjectile>();
        projectile.attackProfile = attackProfile;
        projectile.transform.position = startingPosition;
        projectile.transform.LookAt(enemy.transform.position + new Vector3(0, 1.1f, 0));
        projectile.target = enemy.transform;
        projectile.playerController = playerController;
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

    private void onPlayerStagger(object sender, System.EventArgs e)
    {
        EndArcaneStep();
        if (playerData.currentWeapon == 2 && knifeSpecialAttackOn)
        {
            knifeSpecialAttackOn = false;
            bolts.SetPositions(away, away);
        }
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
    }


    private void OnDisable()
    {
        playerEvents.onPlayerStagger -= onPlayerStagger;
        playerEvents.onClawSpecial -= onClawSpecial;
    }
}
