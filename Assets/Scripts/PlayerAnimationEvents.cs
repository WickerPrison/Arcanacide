using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    //This is the only script that can be referenced directly by the player animations

    PlayerAnimation playerAnimation;
    PlayerController playerController;
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        playerController = GetComponentInParent<PlayerController>();
        playerScript = GetComponentInParent<PlayerScript>();
    }

    //this funciton determines if any enemies were hit by the attack and deals damage accordingly
    public void AttackHit()
    {
        EnemyScript enemyScript;
        Collider[] hitEnemies = Physics.OverlapSphere(playerController.attackPoint.position, 1, playerController.enemyLayers);
        foreach(Collider enemy in hitEnemies)
        {
            enemyScript = enemy.GetComponent<EnemyScript>();
            enemyScript.LoseHealth(playerScript.attackPower);
        }
    }

    public void AttackSmear()
    {
        //playerAnimation.smearAnimator.Play("smear");
        playerAnimation.particleSmear();
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
