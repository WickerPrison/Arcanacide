using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    //This is the only script that can be referenced directly by the player animations

    PlayerAnimation playerAnimation;
    PlayerController playerController;
    PlayerScript playerScript;
    float attackStaminaCost = 20f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        playerController = GetComponentInParent<PlayerController>();
        playerScript = GetComponentInParent<PlayerScript>();
    }

    //this funciton determines if any enemies were hit by the attack and deals damage accordingly
    public void AttackHit(int smearSpeed)
    {
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
            enemyScript.LoseHealth(playerScript.attackPower);
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
}
