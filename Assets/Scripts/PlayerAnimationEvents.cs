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
    BigClaws bigClaws;

    //variables
    bool doLanternCombo;

    private void Awake()
    {
        bigClaws = transform.parent.GetComponentInChildren<BigClaws>();
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
        playerEvents.DashStart();
        playerMovement.gameObject.layer = 8;
        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step))
        {
            patchEffects.StartArcaneStep();
        }

        if(playerData.equippedEmblems.Contains(emblemLibrary.mirror_cloak) && patchEffects.mirrorCloakTimer <= 0)
        {
            playerSound.PlaySoundEffectFromList(11, 0.5f);
            playerEvents.EndMirrorCloak();
            playerAbilities.shield = true;
            playerAbilities.parry = true;
        }
    }

    //Layer 3 is the player layer, it can collide with terrain, enemies, and enemy projectiles
    public void EndIFrames()
    {
        playerEvents.DashEnd();
        playerMovement.gameObject.layer = 3;
        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step))
        {
            patchEffects.EndArcaneStep();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.mirror_cloak) && patchEffects.mirrorCloakTimer <= 0)
        {
            playerAbilities.shield = false;
            playerAbilities.parry = false;
            patchEffects.mirrorCloakTimer = patchEffects.mirrorCloakMaxTime;
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

    public void Backstep(int num)
    {
        playerMovement.gameObject.layer = 8;
        playerEvents.BackstepStart(num);
        Vector3 direction = playerMovement.transform.position - playerMovement.attackPoint.position;
        playerMovement.dashDirection = direction.normalized;
        playerMovement.dashTime = playerMovement.maxDashTime;
        playerSound.Dodge();
    }

    public void EndBackstep()
    {
        playerMovement.gameObject.layer = 3;
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
