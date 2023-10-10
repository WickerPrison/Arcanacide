using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceBreath : MonoBehaviour
{
    ParticleSystem vfx;
    [SerializeField] Transform attackPoint;
    [SerializeField] AttackArcGenerator attackArc;
    [SerializeField] EventReference damageSFX;
    [SerializeField] EventReference fmodEvent;
    EventInstance fmodInstance;
    WaitForSeconds damageSFXdelay = new WaitForSeconds(.6f);
    bool canPlaySound = true;
    EnemyEvents enemyEvents;
    PlayerScript playerScript;
    bool iceBreathOn = false;
    bool hitPlayer;
    float damage = 20;
    float damageCounter;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    // Start is called before the first frame update
    void Start()
    {
        vfx = GetComponent<ParticleSystem>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
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
                RuntimeManager.PlayOneShot(damageSFX);
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
        fmodInstance.start();
    }

    public void StopIceBreath()
    {
        iceBreathOn = false;
        vfx.Stop();
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void OnStagger(object sender, System.EventArgs e)
    {
        StopIceBreath();
    }

    private void OnEnable()
    {
        enemyEvents.OnStagger += OnStagger;
    }

    private void OnDisable()
    {
        enemyEvents.OnStagger -= OnStagger;
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
    }
}
