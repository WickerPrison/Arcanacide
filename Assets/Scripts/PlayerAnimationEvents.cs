using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using FMODUnity;

public class PlayerAnimationEvents : MonoBehaviour
{
    //Input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] ParticleSystem shoveVFX;
    [SerializeField] ParticleSystem electricSmear;
    [SerializeField] GameObject electricTrapPrefab;
    [SerializeField] ParticleSystem icePoof;
    [SerializeField] Animator backAnimator;
    [SerializeField] ExternalLanternFairy lanternFairy;
    [SerializeField] AttackProfiles lanternComboNoFairy;
    [SerializeField] ClawVFX[] clawVFX;
    [SerializeField] PlayerStalagmiteHolder lineStalagmites;
    [SerializeField] PlayerStalagmiteHolder circleStalagmites;
    [SerializeField] Transform[] breathOrigin;

    //player scripts
    PlayerScript playerScript;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    PlayerMovement playerMovement;
    PlayerEvents playerEvents;
    PlayerSound playerSound;
    PatchEffects patchEffects;
    PlayerHealth playerHealth;
    PlayerAttackHitEvents playerAttackHitEvents;

    //other scripts
    Animator frontAnimator;
    IceBreath iceBreath;
    KnifeTrap knifeTrap;
    WeaponManager weaponManager;



    //variables
    bool doLanternCombo;

    private void Awake()
    {
        clawVFX = transform.parent.GetComponentsInChildren<ClawVFX>();
        playerEvents = GetComponentInParent<PlayerEvents>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAbilities = GetComponentInParent<PlayerAbilities>();
        patchEffects = GetComponentInParent<PatchEffects>();
        playerScript = GetComponentInParent<PlayerScript>();
        playerSound = transform.parent.GetComponentInChildren<PlayerSound>();
        playerHealth = GetComponentInParent<PlayerHealth>();
        frontAnimator = gameObject.GetComponent<Animator>();
        weaponManager = GetComponentInParent<WeaponManager>();
        playerAttackHitEvents = GetComponent<PlayerAttackHitEvents>();
        iceBreath = playerScript.gameObject.GetComponentInChildren<IceBreath>();
        lineStalagmites.gameObject.SetActive(true);
        circleStalagmites.gameObject.SetActive(true);
    }

    public void SwitchWeaponSprite(int weaponID)
    {
        weaponManager.ActivateWeaponSprite(weaponID);
        weaponManager.CheckWeaponMagic();
    }

    public void SwordSpecialAttack(AttackHit attackHit)
    {
        int weaponIndex = Utils.GetWeaponIndex(attackHit.weaponCategory);
        AttackProfiles profile = attackHit.GetProfile(playerData.equippedElements[weaponIndex]);
        playerAbilities.SwordSpecialAttack(profile);
    }

    public void LanternHeavy(AttackHit attackHit)
    {
        playerAbilities.LanternHeavy(attackHit.GetProfile(playerData.equippedElements[1]));
    }

    public void LanternCombo2(AttackHit attackHit)
    {
        if (lanternFairy.isInLantern)
        {
            AttackProfiles attackProfile = attackHit.GetProfile(playerData.equippedElements[1]);
            playerScript.LoseStamina(attackProfile.staminaCost);
            doLanternCombo = true;
            playerEvents.LanternCombo();
        }
        else doLanternCombo = false;
    }

    public void EndLanternCombo(AttackProfiles attackProfile)
    {
        if (doLanternCombo)
        {
            playerEvents.EndLanternCombo();
            playerAttackHitEvents.AttackHit(attackProfile);
        }
        else
        {
            Shove();
            playerAttackHitEvents.AttackHit(lanternComboNoFairy);
        }
    }

    public void KnifeHeavy(AttackHit attackHit)
    {
        if (knifeTrap == null)
        {
            knifeTrap = KnifeTrap.Instantiate(attackHit.GetPrefab(playerData.equippedElements[2]), attackHit.GetProfile(playerData.equippedElements[2]), playerScript, playerAbilities);
        }
        else
        {
            knifeTrap.SetDamage();
        }

        knifeTrap.transform.position = transform.parent.position;
        knifeTrap.StartTimer();
    }

    public void KnifeCombo2()
    {
        switch (playerData.equippedElements[2])
        {
            case WeaponElement.ICE:
                circleStalagmites.TriggerWave();
                break;
        }
    }

    public void KnifeSpecialAttack()
    {
        playerAbilities.KnifeSpecialAttack();
    }

    public void ClawSpecialAttack(AttackHit attackHit)
    {
        switch (playerData.equippedElements[3])
        {
            case WeaponElement.ICE:
                lineStalagmites.TriggerWave();
                break;
            case WeaponElement.CHAOS:
                int count = attackHit.GetProfile(WeaponElement.CHAOS).boltNum;
                for(int i = 0; i < count; i++)
                {
                    PlayerBubbles.Instantiate(attackHit.GetPrefab(WeaponElement.CHAOS), playerAbilities.transform.position, playerAbilities);
                }
                break;
        }
    }

    public void AttackFalse()
    {
        playerAnimation.EndChain();
        if (playerScript.testingEvents != null) playerScript.testingEvents.AttackFalse();
    }

    public void EndAttack()
    {
        playerAnimation.attacking = false;
        playerMovement.lockPosition = false;
        playerAnimation.EndChain();
        if (playerScript.testingEvents != null) playerScript.testingEvents.AttackFalse();
    }

    public void Heal()
    {
        playerHealth.GemHeal();
    }

    //Layer 8 is the IFrame layer. It cannot collide with the enemy projectile layer, but otherwise 
    //behaves the same as the player layer
    public void StartIFrames()
    {
        // the isDashing = true and setting layer to 8 has been moved to the Dodge function in PlayerScript due to script execution order problems
        if (playerData.equippedPatches.Contains(Patches.ARCANE_STEP))
        {
            patchEffects.StartArcaneStep();
        }

        if(playerData.equippedPatches.Contains(Patches.MIRROR_CLOAK) && patchEffects.mirrorCloakTimer <= 0)
        {
            //playerSound.PlaySoundEffectFromList(11, 0.5f);
            playerEvents.EndMirrorCloak();
        }
        playerEvents.DashStart();
    }

    //Layer 3 is the player layer, it can collide with terrain, enemies, and enemy projectiles
    public void EndIFrames()
    {
        playerMovement.isDashing = false;
        playerMovement.gameObject.layer = 3;
        if (playerData.equippedPatches.Contains(Patches.ARCANE_STEP))
        {
            patchEffects.EndArcaneStep();
        }

        if (playerData.equippedPatches.Contains(Patches.MIRROR_CLOAK) && patchEffects.mirrorCloakTimer <= 0)
        {
            patchEffects.mirrorCloakTimer = patchEffects.mirrorCloakMaxTime;
        }
        playerEvents.DashEnd();
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
            playerMovement.isDashing = false;
        }
        playerAnimation.attacking = false;
        playerAbilities.shield = true;
        playerAnimation.StartBodyMagic();
    }

    public void EndShield()
    {
        playerAbilities.shield = false;
        playerAbilities.parry = false;
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

    public void StartFloat()
    {
        playerMovement.canWalk = true;
        playerMovement.moveSpeed = playerMovement.floatSpeed;
    }

    public void EndFloat()
    {
        playerMovement.canWalk = false;
        playerMovement.moveSpeed = playerMovement.walkSpeed;
    }

    public void Shove()
    {
        shoveVFX.Play();
    }

    public void BigClaw(AttackProfiles attackProfile)
    {
        clawVFX[0].StartVFX(attackProfile.smearRotations);
    }

    public void DoubleClaw(AttackProfiles attackProfile)
    {
        clawVFX[0].StartVFX(attackProfile.smearRotations);
        clawVFX[1].StartVFX(attackProfile.secondClawRot);
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
        playerSound.PlaySoundEffect(PlayerSFX.FOOTSTEP, 1);
    }

    public void ElectricSmear()
    {
        if(playerData.equippedElements[2] == WeaponElement.ELECTRICITY)
        {
            electricSmear.Play();
        }
    }

    public void KnifeCombo1Vfx()
    {
        Vector3 direction = playerMovement.attackPoint.position - playerMovement.transform.position;
        playerEvents.KnifeCombo1Vfx(direction, playerAnimation.facingFront);
    }

    public void ClawCombo2(AttackHit attackHit)
    {
        AttackProfiles attackProfile = attackHit.GetProfile(playerData.equippedElements[3]);
        switch (attackProfile.element)
        {
            case WeaponElement.ICE:
                IceBreath();
                break;
            case WeaponElement.CHAOS:
                playerScript.LoseStamina(attackProfile.staminaCost);
                int index = playerAnimation.facingDirection > 1 ? 1 : 0;
                Vector3 position = breathOrigin[index].position;
                int count = UnityEngine.Random.Range(1, 12);
                for(int i = 0; i < count; i++)
                {
                    Vector3 direction = playerMovement.attackPoint.position - playerMovement.transform.position;
                    float angle = UnityEngine.Random.Range(-attackProfile.halfConeAngle, attackProfile.halfConeAngle);
                    direction = Utils.RotateDirection(direction, angle);
                    direction.y = UnityEngine.Random.Range(-0.2f, 0.2f);
                    PlayerProjectileStraight.Instantiate(attackHit.GetPrefab(WeaponElement.CHAOS), position, direction.normalized, attackProfile, playerAbilities);
                    RuntimeManager.PlayOneShot(attackProfile.noHitSoundEvent, attackProfile.soundNoHitVolume, transform.position);
                }
                break;
        }
    }

    public void IceBreath()
    {
        playerEvents.IceBreath();
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

    public void MaxCharge()
    {
        switch (playerData.currentWeapon)
        {
            case 0:
                playerEvents.SwordHeavyFullCharge();
                break;
            case 3:
                if (playerScript.testingEvents != null) playerScript.testingEvents.FullyCharged();
                break;
        }
    }

    public void CheckIfCanLanternCombo2()
    {
        if (!lanternFairy.isInLantern) playerAnimation.PlayAnimation("EndAttack3");
    }

    public void Backstep(int num)
    {
        playerMovement.LockAttackPoint();
        playerMovement.gameObject.layer = 8;
        playerEvents.BackstepStart(num);
        Vector3 direction = playerMovement.transform.position - playerMovement.attackPoint.position;
        playerMovement.dashDirection = direction.normalized;
        playerMovement.isDashing = true;
        playerSound.PlaySoundEffect(PlayerSFX.DODGE, 0.5f);
    }

    public void EndBackstep()
    {
        playerMovement.UnlockAttackPoint();
        playerMovement.gameObject.layer = 3;
        playerMovement.isDashing = false;
        playerEvents.DashEnd();
    }

    public void LoseStamina(AttackProfiles profile)
    {
        playerScript.LoseStamina(profile.staminaCost);
    }

    private void onPlayerStagger(object sender, EventArgs e)
    {
        SwitchWeaponSprite(playerData.currentWeapon);
        playerMovement.canWalk = false;
        frontAnimator.SetLayerWeight(1, 0);
        backAnimator.SetLayerWeight(1, 0);
        StartInput();

        EndIceBreath();
        EndFloat();
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
