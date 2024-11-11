using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnightSummon : ChaosSummon
{
    [System.NonSerialized] public ChaosBossController bossController;
    [SerializeField] SpriteRenderer indicatorCircle;
    [SerializeField] Transform jumpPoint;
    [SerializeField] PlayVFX landVFX;
    [SerializeField] ParticleSystem streak;
    CameraFollow cameraScript;
    SpriteEffects spriteEffects;
    WaitForSeconds jumpTime = new WaitForSeconds(0.5f);
    float jumpSpeed = 50;
    [SerializeField] int jumpDamage;
    [SerializeField] float jumpPoiseDamage;
    float indicatorCircleSpeed = 6;
    bool inAir = false;
    Vector3 jumpPointDirection;
    Vector3 indicatorDirection;
    Vector3 upPosition = new Vector3(0, 15, 0);
    
    public override void Start()
    {
        indicatorCircle.transform.parent = null;
        jumpPoint.parent = null;
        spriteEffects = GetComponent<SpriteEffects>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        base.Start();
    }

    public void GetSummoned()
    {
        indicatorCircle.enabled = true;
        indicatorCircle.transform.position = playerScript.transform.position;
        indicatorCircleSpeed = 6;
        jumpPoint.position = playerScript.transform.position + upPosition;
        transform.position = jumpPoint.position;
        inAir = true;
        spriteEffects.SetDissolve(0);
        streak.Play();
        StartCoroutine(HangTime());
    }
    
    private void FixedUpdate()
    {
        if (!inAir) return;

        indicatorDirection = playerScript.transform.position - indicatorCircle.transform.position;

        if (Vector3.Distance(playerScript.transform.position, indicatorCircle.transform.position) > indicatorCircleSpeed * Time.deltaTime)
        {
            indicatorCircle.transform.position += indicatorDirection.normalized * indicatorCircleSpeed * Time.fixedDeltaTime;
        }

        jumpPointDirection = jumpPoint.position - transform.position;
        transform.position += jumpPointDirection.normalized * jumpSpeed * Time.fixedDeltaTime;
        if (indicatorCircleSpeed == 0 && Vector3.Distance(transform.position, jumpPoint.position) <= jumpSpeed * Time.deltaTime)
        {
            transform.position = jumpPoint.position;
            Land();
        }
    }

    IEnumerator HangTime()
    {
        yield return jumpTime;
        indicatorCircleSpeed = 0;
        transform.position = indicatorCircle.transform.position + upPosition;
        jumpPoint.position = indicatorCircle.transform.position;
    }

    void Land()
    {
        frontAnimator.Play("Land");
        inAir = false;
        indicatorCircle.enabled = false;
        StartCoroutine(cameraScript.ScreenShake(.1f, .3f));
        sfx.OtherSounds(0, 3);
        streak.Stop();
        streak.Clear();
        landVFX.PlayParticleSystems();
        if (Vector3.Distance(jumpPoint.position, playerScript.transform.position) <= 3.5f)
        {
            if (playerScript.gameObject.layer == 3)
            {
                sfx.SwordImpact();
                playerScript.LoseHealth(jumpDamage, EnemyAttackType.MELEE, enemyScript);
                playerScript.LosePoise(jumpPoiseDamage);
            }
            else if (playerScript.gameObject.layer == 8)
            {
                playerScript.PerfectDodge(EnemyAttackType.MELEE, enemyScript);
            }
        }
    }

    public override void GoAway()
    {
        bossController.knights.Enqueue(this);
        base.GoAway();
    }
}
