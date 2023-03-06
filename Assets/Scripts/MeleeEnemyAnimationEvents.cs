using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimationEvents : MonoBehaviour
{
    EnemyController enemyController;
    EnemyScript enemyScript;
    [System.NonSerialized] public EnemySound enemySound;
    public AttackArcGenerator attackArc;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        enemyScript = GetComponentInParent<EnemyScript>();
        enemySound = transform.parent.GetComponentInChildren<EnemySound>();
        if(attackArc == null)
        {
            attackArc = transform.parent.GetComponentInChildren<AttackArcGenerator>();
        }
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

    public virtual void EnableNavAgent()
    {
        enemyController.navAgent.enabled = true;
    }

    public void EndAttack()
    {
        enemyController.attacking = false;
    }

    public void AttackHit(int smearSpeed)
    {
        enemyController.canHitPlayer = attackArc.CanHitPlayer();
        enemyController.AttackHit(smearSpeed);
    }

    public void SpecialAbility()
    {
        enemyController.SpecialAbility();
    }

    public void SpecialAbilityOff()
    {
        enemyController.SpecialAbilityOff();
    }

    public void SpellAttack()
    {
        enemyController.SpellAttack();
    }

    public void SpecialEffect()
    {
        enemyController.SpecialEffect();
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

    public void HideArc()
    {
        attackArc.HideAttackArc();
    }

    public void ColorArc()
    {
        attackArc.ColorArc();
    }

    public void Footstep()
    {
        enemySound.Footstep();
    }

    public virtual void Death()
    {
        enemyScript.Death();
    }
}
