using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceBreath : MonoBehaviour
{
    ParticleSystem vfx;
    AudioSource sfx;
    [SerializeField] Transform attackPoint;
    [SerializeField] AttackArcGenerator attackArc;
    [SerializeField] AudioClip damageSFX;
    WaitForSeconds damageSFXdelay = new WaitForSeconds(.6f);
    bool canPlaySound = true;
    EnemyScript enemyScript;
    PlayerScript playerScript;
    bool iceBreathOn = false;
    bool hitPlayer;
    float damage = 20;
    float damageCounter;

    private void Awake()
    {
        enemyScript = GetComponentInParent<EnemyScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        vfx = GetComponent<ParticleSystem>();
        sfx = GetComponent<AudioSource>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if (!iceBreathOn) return;

        Vector3 lookDirection = attackPoint.position - transform.parent.position;
        lookDirection = new Vector3(lookDirection.x, 0, lookDirection.z).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        hitPlayer = attackArc.CanHitPlayer();

        if(hitPlayer )
        {
            if (canPlaySound)
            {
                sfx.PlayOneShot(damageSFX);
                StartCoroutine(SFXtimer());
            }

            damageCounter += damage * Time.deltaTime;
            if(damageCounter > 1)
            {
                playerScript.LoseHealth(Mathf.RoundToInt(damageCounter), EnemyAttackType.NONPARRIABLE, null);
                playerScript.LoseStamina(damageCounter * 3);
                damageCounter = 0;
            }
        }
    }

    IEnumerator SFXtimer()
    {
        canPlaySound = false;
        yield return damageSFXdelay;
        canPlaySound = true;
    }

    public void StartIceBreath()
    {
        iceBreathOn = true;
        vfx.Play();
        sfx.Play();
    }

    public void StopIceBreath()
    {
        iceBreathOn = false;
        vfx.Stop();
        sfx.Stop();
    }

    void OnStagger(object sender, System.EventArgs e)
    {
        StopIceBreath();
    }

    private void OnEnable()
    {
        enemyScript.OnStagger += OnStagger;
    }
}
