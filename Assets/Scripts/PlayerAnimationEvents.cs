using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    //This is the only script that can be referenced directly by the player animations
    [SerializeField] PlayerData playerData;
    CameraFollow cameraScript;
    PlayerAnimation playerAnimation;
    PlayerController playerController;
    PlayerScript playerScript;
    PlayerSound playerSound;
    Animator frontAnimator;
    float attackStaminaCost = 20f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        playerController = GetComponentInParent<PlayerController>();
        playerScript = GetComponentInParent<PlayerScript>();
        playerSound = transform.parent.GetComponentInChildren<PlayerSound>();
        frontAnimator = gameObject.GetComponent<Animator>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    //this funciton determines if any enemies were hit by the attack and deals damage accordingly
    public void AttackHit(int smearSpeed)
    {
        playerSound.SwordSwoosh();
        playerAnimation.parryWindow = false;
        playerScript.LoseStamina(attackStaminaCost);
        ParticleSystem.ShapeModule smearShapeBack = playerAnimation.backSmear.shape;
        smearShapeBack.arcSpeed = -smearSpeed;
        ParticleSystem.ShapeModule smearShapeFront = playerAnimation.frontSmear.shape;
        smearShapeFront.arcSpeed = smearSpeed;
        playerAnimation.particleSmear();
        EnemyScript enemyScript;
        Collider[] getHitEnemies = playerController.HitBox();
        Collider[] hitEnemies = getHitEnemies;
        foreach(Collider enemy in hitEnemies)
        {
            enemyScript = enemy.GetComponent<EnemyScript>();
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController.parryWindow && enemyController.canHitPlayer)
            {
                enemyController.isParrying = true;
            }
            else if(playerAnimation.isParrying)
            {
                playerAnimation.isParrying = false;
                playerController.Parry(enemyScript);
            }
            else
            {
                playerSound.SwordImpact();
                StartCoroutine(cameraScript.ScreenShake(.1f, .03f));
                enemyScript.LoseHealth(playerController.AttackPower(), playerController.AttackPower());
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
}
