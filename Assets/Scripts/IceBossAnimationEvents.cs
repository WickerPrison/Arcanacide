using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceBossAnimationEvents : MeleeEnemyAnimationEvents
{
    [SerializeField] BossIceBreath iceBreath;
    [SerializeField] FacePlayerSlow facePlayer;
    [SerializeField] GameObject iceRipplePrefab;
    [SerializeField] ParticleSystem ringBlast1VFX;
    [SerializeField] GameObject ringBlast1Circle;
    [SerializeField] ParticleSystem ringBlastVFX2;
    [SerializeField] GameObject ringBlast2Circle;
    [SerializeField] SpriteRenderer[] humanLimbs;
    [SerializeField] SpriteRenderer[] golemLimbs;
    [SerializeField] float delayTime;
    WaitForSeconds ringBlastDelay;
    IceBoss bossController;

    public override void Start()
    {
        base.Start();
        ringBlastDelay = new WaitForSeconds(delayTime);
        bossController = GetComponentInParent<IceBoss>();

        for(int i = 0; i < golemLimbs.Length; i++)
        {
            humanLimbs[i].enabled = true;
            golemLimbs[i].enabled = false;
        }
    }

    public override void ChangeArc(AnimationEvent angleAndRadius)
    {
        if(bossController.currentLimb > 2)
        {
            angleAndRadius.floatParameter += 1;
            angleAndRadius.intParameter += 15;
        }
        base.ChangeArc(angleAndRadius);
    }

    public void StartIceBreath()
    {
        iceBreath.StartIceBreath();
    }

    public void StopIceBreath()
    {
        iceBreath.StopIceBreath();
    }

    public void SetNavAgentSpeed(float speed)
    {
        bossController.navAgent.speed = speed;
    }

    public void SetRotationSpeed(int speed)
    {
        facePlayer.rotateSpeed = speed;
    }

    public void ReplaceLimb(int limb)
    {
        humanLimbs[limb].enabled = false;
        golemLimbs[limb].enabled = true;
        if(limb == 4)
        {
            humanLimbs[5].enabled = false;
        }
    }

    public void Smash()
    {
        bossController.canHitPlayer = attackArc.CanHitPlayer();
        bossController.Smash();
    }

    public void Stomp()
    {
        enemySound.OtherSounds(1, 1);
        GameObject iceRipple = Instantiate(iceRipplePrefab);
        iceRipple.transform.position = bossController.transform.position + new Vector3(0, 1, 0);
    }

    public void DoneTransforming()
    {
        bossController.state = EnemyState.IDLE;
        bossController.justTransformed = true;
        bossController.attackTime = 1;
    }

    public override void EnableMovement()
    {
        base.EnableMovement();
        bossController.navAgent.speed = 3;
    }

    public void RingBlast1ShowCircle()
    {
        ringBlast1Circle.SetActive(true);
    }

    public void RingBlast1()
    {
        ringBlast1VFX.Play();
        StartCoroutine(RingBlast1Finish());
    }

    IEnumerator RingBlast1Finish()
    {
        yield return ringBlastDelay;
        ringBlast1Circle.SetActive(false);
        ringBlast2Circle.SetActive(true);
        bossController.RingBlast(0, 6);
    }

    public void RingBlast2()
    {
        ringBlastVFX2.Play();
        StartCoroutine(RingBlast2Finish());
    }

    IEnumerator RingBlast2Finish()
    {
        yield return ringBlastDelay;
        ringBlast2Circle.SetActive(false);
        bossController.RingBlast(6, 15);
    }

    public void BecomeInvincible()
    {
        EnemyScript enemyScript = bossController.GetComponent<EnemyScript>();
        enemyScript.invincible = true;
        bossController.attackMaxTime = 2;
        StartCoroutine(bossController.InvincibleTimer());
    }
}
