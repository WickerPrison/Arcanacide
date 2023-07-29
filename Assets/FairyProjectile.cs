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
    AudioSource sfx;
    ParticleSystem trail;
    GameManager gm;
    Transform target;
    Vector3 offset = new Vector3(0, 1.5f, 0);
    Vector3 initialPos;
    float speed = 30;
    float range = 7;
    bool stop = false;

    private void Start()
    {
        trail = gameObject.GetComponent<ParticleSystem>();
        sfx = GetComponent<AudioSource>();
        lanternFairy.ToggleSprites(false);
        initialPos = transform.position;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        float closestDistance = range;
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
        if(Vector3.Distance(initialPos, transform.position) > range) Return();
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
        StartCoroutine(SelfDestruct());
    }

    public IEnumerator SelfDestruct()
    {
        stop = true;
        yield return new WaitForSeconds(.5f);
        explosion.Play();
        int damage = playerAbilities.DetermineAttackDamage(axeHeavyProfile);
        sfx.PlayOneShot(axeHeavyProfile.soundNoHit, axeHeavyProfile.soundNoHitVolume);
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
