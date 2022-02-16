using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    //This is the only script that can be referenced directly by the player animations
    [SerializeField] PlayerData playerData;
    PlayerAnimation playerAnimation;
    PlayerController playerController;
    PlayerScript playerScript;
    Animator frontAnimator;
    float attackStaminaCost = 20f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        playerController = GetComponentInParent<PlayerController>();
        playerScript = GetComponentInParent<PlayerScript>();
        frontAnimator = gameObject.GetComponent<Animator>();
    }

    //this funciton determines if any enemies were hit by the attack and deals damage accordingly
    public void AttackHit(int smearSpeed)
    {
        playerAnimation.parryWindow = false;
        playerScript.LoseStamina(attackStaminaCost);
        ParticleSystem.ShapeModule smearShapeBack = playerAnimation.backSmear.shape;
        smearShapeBack.arcSpeed = -smearSpeed;
        ParticleSystem.ShapeModule smearShapeFront = playerAnimation.frontSmear.shape;
        smearShapeFront.arcSpeed = smearSpeed;
        playerAnimation.particleSmear();
        EnemyScript enemyScript;
        Collider[] hitEnemies = Physics.OverlapSphere(playerController.attackPoint.position, 1, playerController.enemyLayers);
        foreach(Collider enemy in hitEnemies)
        {
            enemyScript = enemy.GetComponent<EnemyScript>();
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController.parryWindow)
            {
                enemyController.isParrying = true;
            }
            else if(playerAnimation.isParrying)
            {
                playerAnimation.isParrying = false;
            }
            else
            {
                enemyScript.LoseHealth(playerData.attackPower, playerData.attackPower);
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
    }

    //Layer 3 is the player layer, it can collide with terrain, enemies, and enemy projectiles
    public void EndIFrames()
    {
        playerController.gameObject.layer = 3;
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
        playerController.shield = true;
        playerAnimation.StartBodyMagic();
    }

    public void EndShield()
    {
        playerController.shield = false;
        playerAnimation.EndBodyMagic();
    }

    public void ParryWindow()
    {
        if (frontAnimator.GetBool("Attacks"))
        {
            playerAnimation.parryWindow = true;
        }
    }
}
