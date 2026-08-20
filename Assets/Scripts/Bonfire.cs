using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] EventReference impactSound;
    [SerializeField] EventReference fireSound;
    EventInstance fmodInstance;
    public EnemyScript enemyOfOrigin;
    BossController bossController;
    PlayerScript player;
    float duration = 7;
    int damage = 20;
    float poiseDamage = 70;
    float speed = 8;
    Vector3 maxSize = new Vector3(2, 2, 3);
    WaitForEndOfFrame endOfFrame;
    float activationTime = 1;
    bool active = false;

    private void Start()
    {
        fmodInstance = RuntimeManager.CreateInstance(fireSound);
        fmodInstance.start();
        fmodInstance.setTimelinePosition(Random.Range(0, 2000));
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        bossController = enemyOfOrigin.GetComponent<BossController>();
        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        transform.localScale = Vector3.zero;
        float timer = activationTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(maxSize, Vector3.zero, timer / activationTime);
            yield return endOfFrame;
        }
        active = true;
    }

    void CollideWithPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 0.5f) return;
        player.HitPlayer(() =>
        {
            player.LoseHealth(damage, EnemyAttackType.PROJECTILE, enemyOfOrigin);
            player.LosePoise(poiseDamage);
            FmodUtils.PlayOneShot(impactSound, 0.5f, transform.position);
            Destroy(gameObject);
        }, () =>
        {
            player.PerfectDodge(EnemyAttackType.PROJECTILE, enemyOfOrigin, gameObject);
        });
    }

    private void Update()
    {
        if (!active) return;
        CollideWithPlayer();

        duration -= Time.deltaTime;
        if(duration <= 0)
        {
            Destroy(gameObject);
        }

        if (bossController.hasSurrendered)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if(!active) return;

        Vector3 direction = player.transform.position - transform.position;
        transform.Translate(direction.normalized * Time.fixedDeltaTime * speed);
    }

    private void OnDisable()
    {
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
    }
}
