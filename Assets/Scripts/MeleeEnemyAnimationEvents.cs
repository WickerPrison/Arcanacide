using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimationEvents : MonoBehaviour
{
    EnemyController enemyController;
    EnemySound enemySound;
    AttackArcGenerator attackArc;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        enemySound = transform.parent.GetComponentInChildren<EnemySound>();
        attackArc = transform.parent.GetComponentInChildren<AttackArcGenerator>();
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
        enemyController.parryWindow = false;
        enemySound.SwordSwoosh();
        enemyController.AttackHit(smearSpeed);
        attackArc.HideAttackArc();
    }

    public void SpecialAbility()
    {
        enemyController.SpecialAbility();
    }

    public void ParryWindowOn()
    {
        enemyController.parryWindow = true;
    }

    public void ParryWindowOff()
    {
        enemyController.parryWindow = false;
    }

    public void ShowArc()
    {
        attackArc.ShowAttackArc();
    }

    public void ColorArc()
    {
        attackArc.ColorArc();
    }

    public void Footstep()
    {
        enemySound.Footstep();
    }
}
