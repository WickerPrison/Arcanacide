using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyProjectile : MonoBehaviour
{
    [System.NonSerialized] public ExternalLanternFairy lanternFairy;
    [System.NonSerialized] public Vector3 direction;
    [System.NonSerialized] public PlayerAbilities playerAbilities;
    [SerializeField] AttackProfiles axeHeavyProfile;
    [SerializeField] ParticleSystem explosion;
    ParticleSystem trail;
    GameManager gm;
    Transform target;
    Vector3 offset = new Vector3(0, 1.5f, 0);
    Vector3 initialPos;
    float speed = 30;
    float range = 7;
    bool stop = false;
    bool selfDestructed = false;

    private void Start()
    {
        trail = gameObject.GetComponent<ParticleSystem>();
        lanternFairy.ToggleSprites(false);
        initialPos = playerAbilities.transform.position;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        float closestDistance = 100;
        target = null;
        foreach(EnemyScript enemy in gm.enemies)
        {
            float distance = Vector3.Distance(initialPos, enemy.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }

    private void Update()
    {
        if (selfDestructed) return;
        Vector3 currentPos = new Vector3(transform.position.x, 0, transform.position.z);
        if(Vector3.Distance(initialPos, currentPos) > range) StartCoroutine(SelfDestruct());
    }

    private void FixedUpdate()
    {
        trail.Play(false);
        if(target != null)
        {
            direction = target.position + offset - transform.position;
        }
        if (stop) return;
        transform.position += Time.fixedDeltaTime * speed * direction.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (selfDestructed) return;
        StartCoroutine(SelfDestruct());
    }

    public IEnumerator SelfDestruct()
    {
        selfDestructed = true;
        stop = true;
        yield return new WaitForSeconds(.5f);
        explosion.Play();
        int damage = playerAbilities.DetermineAttackDamage(axeHeavyProfile);
        RuntimeManager.PlayOneShot(axeHeavyProfile.noHitSoundEvent, axeHeavyProfile.soundNoHitVolume, transform.position);
        Vector3 groundPosition = new Vector3(transform.position.x, 0, transform.position.z);
        foreach(EnemyScript enemy in gm.enemies)
        {
            if(Vector3.Distance(enemy.transform.position, groundPosition) < axeHeavyProfile.attackRange)
            {
                playerAbilities.DamageEnemy(enemy, damage, axeHeavyProfile);
            }
        }
        yield return new WaitForSeconds(0.7f);
        Return();
    }

    void Return()
    {
        lanternFairy.Return(transform.position);
        Destroy(gameObject);
    }
}
