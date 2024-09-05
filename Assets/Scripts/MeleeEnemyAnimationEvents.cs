using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAnimationEvents : MonoBehaviour
{
    EnemyController enemyController;
    EnemyScript enemyScript;
    [System.NonSerialized] public EnemySound enemySound;
    public AttackArcGenerator attackArc;
    SpriteEffects spriteEffects;
    FacePlayer facePlayer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        enemyScript = GetComponentInParent<EnemyScript>();
        spriteEffects = GetComponentInParent<SpriteEffects>();
        enemySound = transform.parent.GetComponentInChildren<EnemySound>();
        facePlayer = GetComponentInParent<FacePlayer>();
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
        enemyController.state = EnemyState.IDLE;
        enemyController.directionLock = false;
    }

    public virtual void EnableNavAgent()
    {
        enemyController.navAgent.enabled = true;
    }

    public void NavAgentSpeed(float speed)
    {
        enemyController.navAgent.speed = speed;
    }

    public void EndAttack()
    {
        enemyController.state = EnemyState.IDLE;
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

    public virtual void ChangeArc(AnimationEvent angleAndRadius)
    {
        attackArc.ChangeArc(angleAndRadius.intParameter, angleAndRadius.floatParameter);
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

    public void ManualFace()
    {
        facePlayer.ManualFace();
    }

    public void StartDissolve()
    {
        StartCoroutine(spriteEffects.Dissolve());
    }

    public virtual void Death()
    {
        enemyScript.Death();
    }

    public void PauseAudio(int isPaused)
    {
        enemySound.SetPaused(isPaused == 0);
    }

    public void OtherSound(AnimationEvent input)
    {
        enemySound.OtherSounds(input.intParameter, input.floatParameter);
    }
}
