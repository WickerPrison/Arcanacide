using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitEvents : MonoBehaviour
{
    //input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;

    //player scripts
    PlayerScript playerScript;
    PlayerAnimation playerAnimation;
    PlayerAbilities playerAbilities;
    PlayerMovement playerMovement;
    PlayerEvents playerEvents;

    //other scripts
    GameManager gm;
    StepWithAttack stepWithAttack;
    PlayerSmear smear;
    CameraFollow cameraScript;
    PlayerAttackArc attackArc;

    bool charging = false;
    [System.NonSerialized] public float chargeTimer;
    float chargeDecimal;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        playerAnimation = playerScript.GetComponent<PlayerAnimation>();
        playerAbilities = playerScript.GetComponent<PlayerAbilities>();
        playerMovement = playerScript.GetComponent<PlayerMovement>();
        playerEvents = playerScript.GetComponent<PlayerEvents>();

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stepWithAttack = transform.parent.GetComponent<StepWithAttack>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        smear = transform.parent.GetComponentInChildren<PlayerSmear>();
        attackArc = playerMovement.attackPoint.gameObject.GetComponent<PlayerAttackArc>();
    }

    private void Update()
    {
        if (charging)
        {
            chargeTimer += Time.deltaTime;
        }
    }

    public void AttackHitEvent(AttackHit attackHit)
    {
        int weaponIndex = Utils.GetWeaponIndex(attackHit.weaponCategory);
        AttackProfiles profile = attackHit.GetProfile(playerData.equippedElements[weaponIndex]);
        if(profile != null)
        {
            AttackHit(profile);
        }
    }

    //this funciton determines if any enemies were hit by the attack and deals damage accordingly
    public void AttackHit(AttackProfiles attackProfile)
    {
        stepWithAttack.Step(attackProfile.stepWithAttack);

        if (!attackProfile.noHitSoundEvent.IsNull)
        {
            RuntimeManager.PlayOneShot(attackProfile.noHitSoundEvent, attackProfile.soundNoHitVolume);
        }
        playerAnimation.parryWindow = false;
        playerScript.LoseStamina(attackProfile.staminaCost);
        playerScript.LoseMana(attackProfile.manaCost);
        smear.particleSmear(attackProfile);


        if (attackProfile.screenShakeNoHit != Vector2.zero)
        {
            StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeNoHit.x, attackProfile.screenShakeNoHit.y));
        }

        int attackDamage;
        if(attackProfile.chargeDamage > 0)
        {
            attackDamage = playerAbilities.DetermineAttackDamage(attackProfile, chargeDecimal);
        }
        else
        {
            attackDamage = playerAbilities.DetermineAttackDamage(attackProfile);
        }

        switch (attackProfile.hitbox)
        {
            case HitboxType.ARC:
                AttackArcHitbox(attackProfile, attackDamage);
                break;
            case HitboxType.CIRCLE:
                CircleHitbox(attackProfile, attackDamage);
                break;
            case HitboxType.AOE_ZAP:
                AoeZapHitbox(attackProfile, attackDamage);
                break;
        }
    }

    void AttackArcHitbox(AttackProfiles attackProfile, int attackDamage)
    {
        attackArc.ChangeArc(attackProfile);
        attackArc.GetEnemiesInRange();
        foreach (EnemyScript enemy in gm.enemiesInRange)
        {
            playerAbilities.DamageEnemy(enemy, attackDamage, attackProfile);
        }

        if (gm.enemiesInRange.Count > 0 && attackProfile.screenShakeOnHit != Vector2.zero)
        {
            StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeOnHit.x, attackProfile.screenShakeOnHit.y));
        }
    }

    void CircleHitbox(AttackProfiles attackProfile, int attackDamage)
    {
        Utils.CircleHitbox(attackProfile, attackDamage, transform.parent.position, gm, playerAbilities);
    }

    void AoeZapHitbox(AttackProfiles attackProfile, int attackDamage)
    {
        List<Vector3> targets = Utils.HitClosestXEnemies(
            gm.enemies, 
            transform.parent.position, 
            attackProfile.attackRange, 
            attackProfile.boltNum, 
            (enemy) => playerAbilities.DamageEnemy(enemy, attackDamage, attackProfile)
        );
        playerAbilities.KnifeCombo2Vfx(targets);
    }

    public void SwordSwoosh(AttackProfiles attackProfile)
    {
        smear.particleSmear(attackProfile);
    }

    public void Step(float duration)
    {
        stepWithAttack.Step(duration);
    }

    public void SetChargeTimer(float value)
    {
        chargeTimer = value;
    }

    public void StartCharge()
    {
        charging = true;
        if (playerScript.testingEvents != null) playerScript.testingEvents.StartCharging();
    }

    public void ChargeFalse()
    {
        charging = false;
    }

    public float EndCharge(float chargeTime)
    {
        if(chargeTimer > chargeTime)
        {
            chargeDecimal = 1;
        }
        else
        {
            chargeDecimal = chargeTimer / chargeTime;
        }
        return chargeDecimal;
    }

    public void Waterfowl()
    {
        playerEvents.Waterfowl(chargeDecimal);
    }
}
