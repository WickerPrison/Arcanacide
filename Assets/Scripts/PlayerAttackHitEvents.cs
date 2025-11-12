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

    //other scripts
    GameManager gm;
    StepWithAttack stepWithAttack;
    PlayerSmear smear;
    CameraFollow cameraScript;
    PlayerAttackArc attackArc;

    bool charging = false;
    float chargeTimer;
    float chargeDecimal;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        playerAnimation = playerScript.GetComponent<PlayerAnimation>();
        playerAbilities = playerScript.GetComponent<PlayerAbilities>();
        playerMovement = playerScript.GetComponent<PlayerMovement>();

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
        Utils.DrawDebugCircle(12, attackProfile.attackRange, transform.parent.position, 3);
        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.parent.position) < attackProfile.attackRange)
            {
                playerAbilities.DamageEnemy(enemy, attackDamage, attackProfile);
            }
        }

        if (gm.enemiesInRange.Count > 0 && attackProfile.screenShakeOnHit != Vector2.zero)
        {
            StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeOnHit.x, attackProfile.screenShakeOnHit.y));
        }
    }

    void AoeZapHitbox(AttackProfiles attackProfile, int attackDamage)
    {
        List<(EnemyScript, float)> enemiesWithDist = new List<(EnemyScript, float)>();
        foreach(EnemyScript enemy in gm.enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, transform.parent.position);
            if (dist <= attackProfile.attackRange)
            {
                enemiesWithDist.Add((enemy, dist));
            }
        }

        List<Vector3> targets = new List<Vector3>();

        if(enemiesWithDist.Count > 0)
        {
            enemiesWithDist.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            int counter = attackProfile.boltNum;
            while(counter > 0)
            {
                for(int i = 0; i < enemiesWithDist.Count; i++)
                {
                    targets.Add(enemiesWithDist[i].Item1.transform.position + Vector3.up);
                    playerAbilities.DamageEnemy(enemiesWithDist[i].Item1, attackDamage, attackProfile);
                    counter--;
                    if (counter <= 0) break;
                }
            }
        }
        else
        {
            for(int i = 0; i < attackProfile.boltNum; i++)
            {
                float x = Random.Range(-1f, 1f);
                float z = Random.Range(-1f, 1f);
                targets.Add(attackProfile.attackRange / 2 * new Vector3(x, 0, z));
            }
        }
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
}
