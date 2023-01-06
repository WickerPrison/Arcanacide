using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    //This is the only script that can be referenced directly by the player animations
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] ParticleSystem shoveVFX;
    CameraFollow cameraScript;
    PlayerAnimation playerAnimation;
    Smear smear;
    StepWithAttack stepWithAttack;
    PlayerController playerController;
    PlayerScript playerScript;
    PlayerSound playerSound;
    Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    PlayerAttackArc attackArc;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        smear = transform.parent.GetComponentInChildren<Smear>();
        stepWithAttack = transform.parent.GetComponent<StepWithAttack>();
        playerController = GetComponentInParent<PlayerController>();
        playerScript = GetComponentInParent<PlayerScript>();
        playerSound = transform.parent.GetComponentInChildren<PlayerSound>();
        frontAnimator = gameObject.GetComponent<Animator>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        attackArc = playerController.attackPoint.gameObject.GetComponent<PlayerAttackArc>();
    }

    //this funciton determines if any enemies were hit by the attack and deals damage accordingly
    public void AttackHit(AttackProfiles attackProfile)
    {
        stepWithAttack.Step();
        playerSound.SwordSwoosh();
        playerAnimation.parryWindow = false;
        playerScript.LoseStamina(attackProfile.staminaCost);
        smear.particleSmear(attackProfile.smearSpeed);
        attackArc.ChangeArc(attackProfile);

        if(attackProfile.screenShakeNoHit != Vector2.zero)
        {
            StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeNoHit.x, attackProfile.screenShakeNoHit.y));
        }

        int attackDamage = Mathf.RoundToInt(playerController.AttackPower() * attackProfile.damageMultiplier);
        if (playerController.pathActive)
        {
            if (playerData.path == "Sword" || playerData.path == "Dying")
            {
                attackDamage += playerData.PathDamage();
            }
        }
        attackDamage += Mathf.RoundToInt(playerController.MagicalDamage() * attackProfile.magicDamageMultiplier);

        foreach (EnemyScript enemy in gm.enemiesInRange)
        {
            playerSound.SwordImpact();
            if(attackProfile.screenShakeOnHit != Vector2.zero)
            {
                StartCoroutine(cameraScript.ScreenShake(attackProfile.screenShakeOnHit.x, attackProfile.screenShakeOnHit.y));
            }
            enemy.LoseHealth(attackDamage, attackDamage * attackProfile.poiseDamageMultiplier);
            enemy.GainDOT(attackProfile.durationDOT);
        }
    }

    public void SwordHeavy(AttackProfiles attackProfile)
    {
        playerScript.LoseStamina(attackProfile.staminaCost);
        shoveVFX.Play();
        StartCoroutine(cameraScript.ScreenShake(.1f, .3f));
        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < attackProfile.attackRange)
            {
                enemy.LoseHealth(Mathf.RoundToInt(playerController.AttackPower() * attackProfile.damageMultiplier), playerController.AttackPower() * attackProfile.poiseDamageMultiplier);
                EnemyController enemyController = enemy.gameObject.GetComponent<EnemyController>();
                enemyController.StartStagger(0.5f);
            }
        }
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
    }

    //Layer 8 is the IFrame layer. It cannot collide with the enemy projectile layer, but otherwise 
    //behaves the same as the player layer
    public void StartIFrames()
    {
        playerController.gameObject.layer = 8;
        playerController.StartPathOfThePath();
    }

    //Layer 3 is the player layer, it can collide with terrain, enemies, and enemy projectiles
    public void EndIFrames()
    {
        playerController.gameObject.layer = 3;
        playerController.EndPathOfThePath();
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
        if(playerController.gameObject.layer == 8)
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

    public void StartSwordMagic()
    {
        playerAnimation.StartSwordMagic();
    }

    public void EndSwordMagic()
    {
        playerAnimation.EndSwordMagic();
    }

    public void Shove()
    {
        playerController.ShoveEffect();
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
