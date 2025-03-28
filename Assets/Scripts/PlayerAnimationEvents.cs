using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

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
    [SerializeField] ClawVFX clawVFX;

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
    ElectricTrap electricTrap;
    WeaponManager weaponManager;

    //variables
    bool doLanternCombo;

    private void Awake()
    {
        clawVFX = transform.parent.GetComponentInChildren<ClawVFX>();
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
        playerAbilities.AxeHeavy();
    }

    public void LanternCombo()
    {
        if (lanternFairy.isInLantern)
        {
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
        playerMovement.lockPosition = false;
        playerAnimation.EndChain();
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
            playerAbilities.shield = true;
            playerAbilities.parry = true;
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
            playerAbilities.shield = false;
            playerAbilities.parry = false;
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
        clawVFX.StartVFX(attackProfile);
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

    public void MaxCharge()
    {
        playerEvents.SwordHeavyFullCharge();
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
