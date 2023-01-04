using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    //This is the only script that can be referenced directly by the player animations
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    CameraFollow cameraScript;
    PlayerAnimation playerAnimation;
    Smear smear;
    StepWithAttack stepWithAttack;
    PlayerController playerController;
    PlayerScript playerScript;
    PlayerSound playerSound;
    Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    float attackStaminaCost = 20f;
    PlayerAttackArc attackArc;

    // Start is called before the first frame update
    void Start()
    {
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
        playerScript.LoseStamina(attackStaminaCost);
        smear.particleSmear(attackProfile.smearSpeed);
        attackArc.ChangeArc(attackProfile);
        EnemyScript enemyScript;
        foreach(Collider enemy in playerController.enemiesInRange)
        {
            enemyScript = enemy.GetComponent<EnemyScript>();
            playerSound.SwordImpact();
            StartCoroutine(cameraScript.ScreenShake(.1f, .03f));
            enemyScript.LoseHealth(playerController.AttackPower() * attackProfile.damageMultiplier, playerController.AttackPower() * attackProfile.poiseDamageMultiplier);
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
