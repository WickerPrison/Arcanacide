using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HalfGolemController : EnemyController
{
    CameraFollow cameraScript;
    FacePlayer facePlayer;
    float smashRange = 3;
    int remainingIce = 3;
    [SerializeField] List<GameObject> ice = new List<GameObject>();
    [SerializeField] GameObject[] ikManagers;
    [SerializeField] ParticleSystem poof;
    AttackArcGenerator attackArc;
    StepWithAttack stepWithAttack;
    float unfrozenAttackMaxTime = 3;

    public override void Start()
    {
        base.Start();
        navAgent.enabled = false;
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        facePlayer = GetComponent<FacePlayer>();
        attackArc = GetComponentInChildren<AttackArcGenerator>();
        stepWithAttack = GetComponent<StepWithAttack>();
        foreach(GameObject ikManager in ikManagers)
        {
            ikManager.SetActive(false);
        }
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (hasSeenPlayer && !attacking)
        {
            if (attackTime > 0)
            {
                attackTime -= Time.deltaTime;
            }

            if (remainingIce > 0)
            {
                FrozenAI();
            }
            else
            {
                UnfrozenAI();
            }
        }
    }

    void FrozenAI()
    {
        //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
        if (navAgent.enabled == true)
        {
            navAgent.SetDestination(playerController.transform.position);
            return;
        }

        if (Vector3.Distance(playerScript.transform.position, transform.position) < smashRange)
        {
            if (attackTime <= 0)
            {
                attackTime = attackMaxTime;
                attacking = true;
                frontAnimator.Play("Attack" + remainingIce.ToString());
            }
        }
        else
        {
            frontAnimator.Play("Walk");
        }
    }

    void UnfrozenAI()
    {
        if (navAgent.enabled)
        {
            navAgent.SetDestination(playerController.transform.position);
        }

        if (attackTime <= 0 && Vector3.Distance(playerScript.transform.position, transform.position) < smashRange)
        {
            attackTime = unfrozenAttackMaxTime;
            attacking = true;
            frontAnimator.Play("DoubleAttack");
        }
    }

    public override void AttackHit(int smearSpeed)
    {
        smearScript.particleSmear(smearSpeed);
        if(remainingIce == 0)
        {
            stepWithAttack.Step(0.15f);
        }
        //enemySound.OtherSounds(1, 1);
        parryWindow = false;

        if (!canHitPlayer)
        {
            return;
        }

        if (playerController.gameObject.layer == 3)
        {
            playerScript.LoseHealth(hitDamage);
            playerScript.LosePoise(hitPoiseDamage);
        }
        else if (playerController.gameObject.layer == 8)
        {
            playerController.PerfectDodge();
        }
    }

    public override void SpellAttack()
    {
        //enemySound.OtherSounds(0, 1);
        poof.Play();

        if (!attackArc.CanHitPlayer()) 
        {
            return;
        }


        if (playerController.gameObject.layer == 3)
        {
            playerScript.LoseStamina(60);
        }
        else if (playerController.gameObject.layer == 8)
        {
            playerController.PerfectDodge();
        }
    }

    public override void SpecialAbility()
    {
        StartCoroutine(cameraScript.ScreenShake(.1f, .1f));
        //enemySound.OtherSounds(1, 1);
        parryWindow = false;

        if (!canHitPlayer)
        {
            return;
        }


        if (playerController.gameObject.layer == 3)
        {
           // playerScript.LoseHealth(hitDamage);
           // playerScript.LosePoise(hitPoiseDamage);
        }
        else if (playerController.gameObject.layer == 8)
        {
            playerController.PerfectDodge();
        }
    }

    public override void OnHit()
    {
        base.OnHit();

        if(remainingIce > 0)
        {
            ice[0].SetActive(false);
            ice.RemoveAt(0);
            remainingIce--;
            if (remainingIce == 0)
            {
                foreach (GameObject ikManager in ikManagers)
                {
                    ikManager.SetActive(true);
                }

                frontAnimator.SetBool("IsFrozen", false);
                navAgent.enabled = true;
                navAgent.speed = 5;
                attackTime = 0;
            }
        }
    }

    public override void EndStagger()
    {
        base.EndStagger();
        if(remainingIce > 0)
        {
            navAgent.enabled = false;
        }
    }
}
