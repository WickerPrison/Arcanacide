using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationEvents : MonoBehaviour
{
    //This is the only script that can be called by enemy animations
    //It will be inherited by the BossAnimationEvents script and many enemy types

    EnemyController enemyController;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    public void SpellAttack()
    {
        enemyController.SpellAttack();
    }

    public void SpellAttack2()
    {
        enemyController.SpellAttack2();
    }

    //disabling the navAgent prevents the enemy from being able to walk
    public void DisableMovement()
    {
        enemyController.navAgent.enabled = false;
    }

    //enabling the navAgent allows the enemy to walk. Once it is enabled here it is controlled
    //in the EnemyController script
    public virtual void EnableMovement()
    {
        enemyController.navAgent.enabled = true;
    }
}
