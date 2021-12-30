using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimationEvents : MonoBehaviour
{
    EnemyController enemyController;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
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
        enemyController.attacking = false;
        enemyController.directionLock = false;
    }

    public void AttackHit(int smearSpeed)
    {
        enemyController.AttackHit(smearSpeed);
    }

    public void SpecialAbility()
    {
        enemyController.SpecialAbility();
    }
}
