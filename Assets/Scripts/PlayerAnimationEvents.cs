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
    PlayerSmear smear;
    StepWithAttack stepWithAttack;
    PlayerController playerController;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        smear = transform.parent.GetComponentInChildren<PlayerSmear>();
        stepWithAttack = transform.parent.GetComponent<StepWithAttack>();
        playerController = GetComponentInParent<PlayerController>();
        playerScript = GetComponentInParent<PlayerScript>();
        playerSound = transform.parent.GetComponentInChildren<PlayerSound>();
        frontAnimator = gameObject.GetComponent<Animator>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        attackArc = playerController.attackPoint.gameObject.GetComponent<PlayerAttackArc>();
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

        int attackDamage = Mathf.RoundToInt(playerController.AttackPower() * attackProfile.damageMultiplier);
        attackDamage = EmblemDamageModifiers(attackDamage);
        attackDamage += Mathf.RoundToInt(playerData.ArcaneDamage() * attackProfile.magicDamageMultiplier);

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

        enemy.LoseHealth(attackDamage, attackDamage * attackProfile.poiseDamageMultiplier);

        if (attackProfile.heavyAttack && playerData.equippedEmblems.Contains(emblemLibrary.rending_blows))
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
            AttackHitEachEnemy(enemy, attackDamage, attackProfile);
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
                AttackHitEachEnemy(enemy, attackDamage, attackProfile);
            }
        }

        if (gm.enemiesInRange.Count > 0 && attackProfile.screenShakeOnHit != Vector2.zero)
        {
            StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeOnHit.x, attackProfile.screenShakeOnHit.y));
        }
    }

    int EmblemDamageModifiers(int attackDamage)
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary.close_call) && playerController.closeCallTimer > 0)
        {
            attackDamage += emblemLibrary.CloseCallDamage();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_remains) && playerController.arcaneRemainsActive)
        {
            attackDamage += emblemLibrary.ArcaneRemainsDamage();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.confident_killer) && playerData.health == playerData.MaxHealth())
        {
            attackDamage += emblemLibrary.ConfidentKillerDamage();
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
        playerController.SwordSpecialAttack();
    }

    public void AxeHeavy()
    {
        playerController.axeHeavyTimer = playerController.axeHeavyMaxTime;
    }

    public void AxeSpecialAttack()
    {
        playerController.AxeSpecialAttack();
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
        playerController.KnifeSpecialAttack();
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
        playerController.lockPosition = false;
    }

    public void Heal()
    {
        playerScript.Heal();
    }

    //Layer 8 is the IFrame layer. It cannot collide with the enemy projectile layer, but otherwise 
    //behaves the same as the player layer
    public void StartIFrames()
    {
        playerController.gameObject.layer = 8;
        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step))
        {
            playerController.StartArcaneStep();
        }
    }

    //Layer 3 is the player layer, it can collide with terrain, enemies, and enemy projectiles
    public void EndIFrames()
    {
        playerController.gameObject.layer = 3;
        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step))
        {
            playerController.EndArcaneStep();
        }
    }

    public void LockPosition()
    {
        playerController.lockPosition = true;
    }

    public void UnlockPosition()
    {
        playerController.lockPosition = false;
    }

    public void StopInput()
    {
        playerController.preventInput = true;
    }

    public void StartInput()
    {
        playerController.preventInput = false;
    }

    public void StartShield()
    {
        if (playerController.gameObject.layer == 8)
        {
            EndIFrames();
            playerController.dashTime = 0;
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
        playerController.canWalk = true;
        frontAnimator.SetLayerWeight(1, 1);
        backAnimator.SetLayerWeight(1, 1);
    }

    public void EndWalkLayer()
    {
        playerController.canWalk = false;
        frontAnimator.SetLayerWeight(1, 0);
        backAnimator.SetLayerWeight(1, 0);
        if (playerController.moveDirection.magnitude > 0)
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
        playerController.dodgeVFX.Play();
        Vector3 direction = playerController.transform.position - playerController.attackPoint.position;
        playerController.dashDirection = direction.normalized;
        playerController.dashTime = playerController.maxDashTime * 2 / 3;
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
}
