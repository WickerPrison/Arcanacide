using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAnimationEvents : EnemyAnimationEvents
{
    BossController bossController;
    BossDialogue bossDialogue;
    [SerializeField] ParticleSystem deathFire;

    public override void Start()
    {
        base.Start();
        bossController = GetComponentInParent<BossController>();
        bossDialogue = GetComponentInParent<BossDialogue>();
    }

    public void FireTral()
    {
        bossController.FireTrail();
    }

    public override void EnableMovement()
    {
        base.EnableMovement();
        int leftOrRight = Random.Range(1, 3);
        if(leftOrRight == 1)
        {
            bossController.strafeLeftOrRight *= -1;
        }
        bossController.pauseTimer = false;
    }

    public void CanStagger()
    {
        bossController.canStagger = true;
    }

    public void CanNotStagger()
    {
        bossController.canStagger = false;
    }

    public void Bonfire()
    {
        bossController.Bonfire();
    }

    public void FireWave()
    {
        bossController.FireWave();
    }

    public void Phase2FireWave()
    {
        if(bossController.phase > 1)
        {
            FireWave();
        }
    }

    public void Phase1FireWave()
    {
        if(bossController.phase == 1)
        {
            FireWave();
        }
    }

    public void FireRing()
    {
        bossController.FireRing();
    }

    public void GroundFire()
    {
        bossController.GroundFire();
    }

    public void EndStagger()
    {
        bossController.EndStagger();
    }

    public void LookUpDialogue()
    {
        bossDialogue.LookUpDialogue();
    }

    public void EndLookUpDialogue()
    {
        bossDialogue.EndLookUpDialogue();
    }

    public void StartDying()
    {
        deathFire.Play();
    }

    public void StopDeathFire()
    {
        deathFire.Stop();
    }

    public void SurrenderDialogue()
    {
        EnemyScript enemyScript = GetComponentInParent<EnemyScript>();
        enemyScript.health = 0;
        enemyScript.GainHealth(5);
        Dialogue dialogue = GetComponent<Dialogue>();
        dialogue.StartConversation();
        enemyScript.isDying = false;
        bossController.isDisabled = true;
        bossController.hasSurrendered = true;  
    }


    public override void Death()
    {
        base.Death();
        bossController.Death();
    }
}
