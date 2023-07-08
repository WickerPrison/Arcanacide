using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlayerAnimationEvents : MonoBehaviour
{
    //This is the only script that can be referenced directly by the player animations
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] ParticleSystem shoveVFX;
    [SerializeField] ParticleSystem electricSmear;
    [SerializeField] GameObject electricTrapPrefab;
    [SerializeField] ParticleSystem icePoof;
    IceBreath iceBreath;
    ElectricTrap electricTrap;
    CameraFollow cameraScript;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    PlayerEvents playerEvents;
    PlayerSmear smear;
    StepWithAttack stepWithAttack;
    PlayerMovement playerMovement;
    EmblemEffects emblemEffects;
    PlayerScript playerScript;
    PlayerSound playerSound;
    Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    PlayerAttackArc attackArc;
    GameManager gm;
    AudioSource SFX;
    WeaponManager weaponManager;
    BigClaws bigClaws;

    private void Awake()
    {
        bigClaws = transform.parent.GetComponentInChildren<BigClaws>();
        playerEvents = GetComponentInParent<PlayerEvents>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        smear = transform.parent.GetComponentInChildren<PlayerSmear>();
        stepWithAttack = transform.parent.GetComponent<StepWithAttack>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAbilities = GetComponentInParent<PlayerAbilities>();
        emblemEffects = GetComponentInParent<EmblemEffects>();
        playerScript = GetComponentInParent<PlayerScript>();
        playerSound = transform.parent.GetComponentInChildren<PlayerSound>();
        frontAnimator = gameObject.GetComponent<Animator>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        attackArc = playerMovement.attackPoint.gameObject.GetComponent<PlayerAttackArc>();
        SFX = transform.parent.GetComponentInChildren<AudioSource>();
        weaponManager = GetComponentInParent<WeaponManager>();
        iceBreath = playerScript.gameObject.GetComponentInChildren<IceBreath>();
    }

    //this funciton determines if any enemies were hit by the attack and deals damage accordingly
    public void AttackHit(AttackProfiles attackProfile)
    {
        stepWithAttack.Step(attackProfile.stepWithAttack);

        if (attackProfile.soundNoHit != null)
        {
            SFX.PlayOneShot(attackProfile.soundNoHit, attackProfile.soundNoHitVolume);
        }
        playerAnimation.parryWindow = false;
        playerScript.LoseStamina(attackProfile.staminaCost);
        smear.particleSmear(attackProfile);


        if (attackProfile.screenShakeNoHit != Vector2.zero)
        {
            StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeNoHit.x, attackProfile.screenShakeNoHit.y));
        }

        int attackDamage = Mathf.RoundToInt(playerData.AttackPower() * attackProfile.damageMultiplier);
        attackDamage = EmblemDamageModifiers(attackDamage);
        attackDamage += Mathf.RoundToInt(playerData.ArcaneDamage() * attackProfile.magicDamageMultiplier);
        attackDamage = playerAbilities.DamageModifiers(attackDamage);

        switch (attackProfile.hitboxType)
        {
            case "Arc":
                AttackArcHitbox(attackProfile, attackDamage);
                break;
            case "Circle":
                CircleHitbox(attackProfile, attackDamage);
                break;
        }
    }

    void AttackHitEachEnemy(EnemyScript enemy, int attackDamage, AttackProfiles attackProfile)
    {
        if (attackProfile.soundOnHit != null)
        {
            SFX.PlayOneShot(attackProfile.soundOnHit, attackProfile.soundOnHitVolume);
        }

        if (enemy.DOT > 0 && playerData.equippedEmblems.Contains(emblemLibrary.opportune_strike))
        {
            attackDamage = Mathf.RoundToInt(attackDamage * 1.2f);
        }

        enemy.LoseHealth(attackDamage, attackDamage * attackProfile.poiseDamageMultiplier);
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

    void AttackArcHitbox(AttackProfiles attackProfile, int attackDamage)
    {
        attackArc.ChangeArc(attackProfile);
        attackArc.GetEnemiesInRange();
        foreach (EnemyScript enemy in gm.enemiesInRange)
        {
            if (!enemy.blockAttack)
            {
                AttackHitEachEnemy(enemy, attackDamage, attackProfile);
            }
            else
            {
                enemy.GetComponentInChildren<EnemySound>().BlockAttack();
            }
        }


        if (gm.enemiesInRange.Count > 0 && attackProfile.screenShakeOnHit != Vector2.zero)
        {
            StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeOnHit.x, attackProfile.screenShakeOnHit.y));
        }
    }

    void CircleHitbox(AttackProfiles attackProfile, int attackDamage)
    {
        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.parent.position) < attackProfile.attackRange)
            {
                if (!enemy.blockAttack)
                {
                    AttackHitEachEnemy(enemy, attackDamage, attackProfile);
                }
                else
                {
                    enemy.GetComponentInChildren<EnemySound>().BlockAttack();
                }
            }
        }

        if (gm.enemiesInRange.Count > 0 && attackProfile.screenShakeOnHit != Vector2.zero)
        {
            StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeOnHit.x, attackProfile.screenShakeOnHit.y));
        }
    }

    int EmblemDamageModifiers(int attackDamage)
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary.close_call) && emblemEffects.closeCallTimer > 0)
        {
            attackDamage += emblemLibrary.CloseCallDamage();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains) && emblemEffects.arcaneRemainsActive)
        {
            attackDamage += emblemLibrary.ArcaneRemainsDamage();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.confident_killer) && playerData.health == playerData.MaxHealth())
        {
            attackDamage += emblemLibrary.ConfidentKillerDamage();
        }

        if(playerData.equippedEmblems.Contains(emblemLibrary._spellsword) && playerData.mana > emblemLibrary.spellswordManaCost)
        {
            attackDamage += emblemLibrary.SpellswordDamage();
            playerScript.LoseMana(emblemLibrary.spellswordManaCost);
        }

        return attackDamage;
    }

    public void SwitchWeaponSprite(int weaponID)
    {
        weaponManager.ActivateWeaponSprite(weaponID);
        weaponManager.CheckWeaponMagic();
    }

    public void SwordSpecialAttack()
    {
        playerAbilities.SwordSpecialAttack();
    }

    public void AxeHeavy()
    {
        playerAbilities.axeHeavyTimer = playerAbilities.axeHeavyMaxTime;
    }

    public void AxeSpecialAttack()
    {
        playerAbilities.AxeSpecialAttack();
    }

    public void KnifeHeavy()
    {
        if (electricTrap == null)
        {
            electricTrap = Instantiate(electricTrapPrefab).GetComponent<ElectricTrap>();
        }

        electricTrap.transform.position = transform.parent.position;
        electricTrap.StartTimer();
    }

    public void KnifeSpecialAttack()
    {
        playerAbilities.KnifeSpecialAttack();
    }

    public void ClawsSpecialAttack()
    {
        Shove();
        playerEvents.ClawSpecialAttack();
    }

    public void AttackFalse()
    {
        playerAnimation.EndChain();
    }

    public void EndAttack()
    {
        playerAnimation.attacking = false;
        frontAnimator.speed = 1;
        backAnimator.speed = 1;
        playerMovement.lockPosition = false;
    }

    public void Heal()
    {
        playerScript.Heal();
    }

    //Layer 8 is the IFrame layer. It cannot collide with the enemy projectile layer, but otherwise 
    //behaves the same as the player layer
    public void StartIFrames()
    {
        playerEvents.DashStart();
        playerMovement.gameObject.layer = 8;
        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step))
        {
            emblemEffects.StartArcaneStep();
        }

        if(playerData.equippedEmblems.Contains(emblemLibrary.mirror_cloak) && emblemEffects.mirrorCloakTimer <= 0)
        {
            playerSound.PlaySoundEffectFromList(11, 0.5f);
            playerEvents.EndMirrorCloak();
            playerScript.shield = true;
            playerScript.parry = true;
        }
    }

    //Layer 3 is the player layer, it can collide with terrain, enemies, and enemy projectiles
    public void EndIFrames()
    {
        playerEvents.DashEnd();
        playerMovement.gameObject.layer = 3;
        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step))
        {
            emblemEffects.EndArcaneStep();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.mirror_cloak) && emblemEffects.mirrorCloakTimer <= 0)
        {
            playerScript.shield = false;
            playerScript.parry = false;
           emblemEffects.mirrorCloakTimer = emblemEffects.mirrorCloakMaxTime;
        }
    }

    public void LockPosition()
    {
        playerMovement.lockPosition = true;
    }

    public void UnlockPosition()
    {
        playerMovement.lockPosition = false;
    }

    public void StopInput()
    {
        playerMovement.preventInput = true;
    }

    public void StartInput()
    {
        playerMovement.preventInput = false;
    }

    public void StartShield()
    {
        if (playerMovement.gameObject.layer == 8)
        {
            EndIFrames();
            playerMovement.dashTime = 0;
        }
        playerAnimation.attacking = false;
        playerScript.shield = true;
        playerAnimation.StartBodyMagic();
    }

    public void EndShield()
    {
        playerScript.shield = false;
        playerScript.parry = false;
        playerAnimation.EndBodyMagic();
    }

    public void StartWeaponMagic()
    {
        weaponManager.AddWeaponMagicSource();
    }

    public void EndWeaponMagic()
    {
        weaponManager.RemoveWeaponMagicSource();
    }

    public void StartSpecificWeaponMagic(int weaponID)
    {
        weaponManager.AddSpecificWeaponSource(weaponID);
    }

    public void EndSpecificWeaponMagic(int weaponID)
    {
        weaponManager.RemoveSpecificWeaponSource(weaponID);
    }

    public void StartWalkLayer()
    {
        playerMovement.canWalk = true;
        frontAnimator.SetLayerWeight(1, 1);
        backAnimator.SetLayerWeight(1, 1);
    }

    public void EndWalkLayer()
    {
        playerMovement.canWalk = false;
        frontAnimator.SetLayerWeight(1, 0);
        backAnimator.SetLayerWeight(1, 0);
        if (playerMovement.moveDirection.magnitude > 0)
        {
            frontAnimator.Play("Walk", 0, frontAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime);
            backAnimator.Play("Walk", 0, backAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime);
        }
    }

    public void Shove()
    {
        shoveVFX.Play();
    }

    public void BigClaw(AttackProfiles attackProfile)
    {
        bigClaws.ClawSwipe(attackProfile);
    }

    public void ParryWindow()
    {
        if (frontAnimator.GetBool("Attacks"))
        {
            playerAnimation.parryWindow = true;
        }
    }

    public void Footstep()
    {
        playerSound.Footstep();
    }

    public void ElectricSmear()
    {
        electricSmear.Play();
    }

    public void StartIceBreath()
    {
        iceBreath.StartIceBreath();
    }

    public void EndIceBreath()
    {
        iceBreath.StopIceBreath();
    }

    public void IcePoof()
    {
        icePoof.Play();   
    }

    public void Backstep()
    {
        Vector3 direction = playerMovement.transform.position - playerMovement.attackPoint.position;
        playerMovement.dashDirection = direction.normalized;
        playerMovement.dashTime = playerMovement.maxDashTime * 2 / 3;
        playerSound.Dodge();
    }

    public void AttackAnimationSpeed()
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary.quick_strikes))
        {
            frontAnimator.speed = 1.5f;
            backAnimator.speed = 1.5f;
        }
        else
        {
            frontAnimator.speed = 1;
            backAnimator.speed = 1;
        }
    }
    private void onPlayerStagger(object sender, EventArgs e)
    {
        SwitchWeaponSprite(playerData.currentWeapon);
        playerMovement.canWalk = false;
        frontAnimator.SetLayerWeight(1, 0);
        backAnimator.SetLayerWeight(1, 0);
        StartInput();
    }

    private void OnEnable()
    {
        playerEvents.onPlayerStagger += onPlayerStagger;
    }

    private void OnDisable()
    {
        playerEvents.onPlayerStagger -= onPlayerStagger;
    }
}
