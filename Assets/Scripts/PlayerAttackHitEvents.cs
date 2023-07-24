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
    PatchEffects patchEffects;
    PlayerAbilities playerAbilities;
    PlayerMovement playerMovement;

    //other scripts
    GameManager gm;
    AudioSource SFX;
    StepWithAttack stepWithAttack;
    PlayerSmear smear;
    CameraFollow cameraScript;
    PlayerAttackArc attackArc;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        playerAnimation = playerScript.GetComponent<PlayerAnimation>();
        patchEffects = playerScript.GetComponent<PatchEffects>();
        playerAbilities = playerScript.GetComponent<PlayerAbilities>();
        playerMovement = playerScript.GetComponent<PlayerMovement>();

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stepWithAttack = transform.parent.GetComponent<StepWithAttack>();
        SFX = transform.parent.GetComponentInChildren<AudioSource>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        smear = transform.parent.GetComponentInChildren<PlayerSmear>();
        attackArc = playerMovement.attackPoint.gameObject.GetComponent<PlayerAttackArc>();
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

        int attackDamage = playerAbilities.DetermineAttackDamage(attackProfile);

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
}
