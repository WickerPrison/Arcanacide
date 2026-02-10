using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyProjectile : MonoBehaviour
{
    [System.NonSerialized] public ExternalLanternFairy lanternFairy;
    [System.NonSerialized] public Vector3 direction;
    [System.NonSerialized] public PlayerAbilities playerAbilities;
    [SerializeField] ParticleSystem fireExplosion;
    [SerializeField] ParticleSystem lightningExplosion;
    AttackProfiles attackProfile;
    ParticleSystem trail;
    GameManager gm;
    Transform target;
    Vector3 offset = new Vector3(0, 1.5f, 0);
    Vector3 initialPos;
    float speed = 30;
    float range = 7;
    bool stop = false;
    bool selfDestructed = false;
    bool instantiatedCorrectly = false;

    public static FairyProjectile Instantiate(GameObject prefab, Vector3 direction, ExternalLanternFairy lanternFairy, PlayerAbilities playerAbilities, AttackProfiles attackProfile, EnemyScript target)
    {
        FairyProjectile fairyProjectile = Instantiate(prefab).GetComponent<FairyProjectile>();
        fairyProjectile.transform.position = lanternFairy.GetInternalLanternPosition();
        fairyProjectile.direction = direction;
        fairyProjectile.lanternFairy = lanternFairy;
        fairyProjectile.playerAbilities = playerAbilities;
        fairyProjectile.attackProfile = attackProfile;
        fairyProjectile.target = target != null ? target.transform : null;
        fairyProjectile.instantiatedCorrectly = true;
        return fairyProjectile;
    }

    private void Start()
    {
        if (!instantiatedCorrectly)
        {
            Utils.IncorrectInitialization("FairyProjectile");
        }
        trail = gameObject.GetComponent<ParticleSystem>();
        lanternFairy.ToggleSprites(false);
        initialPos = playerAbilities.transform.position;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
        if (selfDestructed || other.CompareTag("Wall")) return;
        StartCoroutine(SelfDestruct());
    }

    public IEnumerator SelfDestruct()
    {
        selfDestructed = true;
        stop = true;
        yield return new WaitForSeconds(.5f);
        PlayExplosion(attackProfile.element);
        int damage = playerAbilities.DetermineAttackDamage(attackProfile);
        FmodUtils.PlayOneShot(attackProfile.noHitSoundEvent, attackProfile.soundNoHitVolume, transform.position);
        Vector3 groundPosition = new Vector3(transform.position.x, 0, transform.position.z);
        foreach(EnemyScript enemy in gm.enemies)
        {
            if(Vector3.Distance(enemy.transform.position, groundPosition) < attackProfile.attackRange)
            {
                playerAbilities.DamageEnemy(enemy, damage, attackProfile);
            }
        }
        yield return new WaitForSeconds(0.7f);
        Return();
    }

    void PlayExplosion(WeaponElement element)
    {
        switch (element)
        {
            case WeaponElement.FIRE:
                fireExplosion.Play();
                break;
            case WeaponElement.ELECTRICITY:
                lightningExplosion.Play();
                break;
        }
    }

    void Return()
    {
        lanternFairy.Return(transform.position);
        Destroy(gameObject);
    }
}
