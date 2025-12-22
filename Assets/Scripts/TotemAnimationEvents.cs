using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemAnimationEvents : MonoBehaviour, IDamageEnemy
{
    [SerializeField] GameObject ripplePrefab;
    [SerializeField] ParticleSystem landingVFX;
    [SerializeField] EventReference landingSFX;
    [SerializeField] EventReference rippleSFX;
    [SerializeField] AttackProfiles lanternSpecialRipple;
    [SerializeField] AttackProfiles zapProfile;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [System.NonSerialized] public ExternalLanternFairy lanternFairy;
    [SerializeField] Transform fairySprite;
    TouchingCollider colliderScript;
    List<Collider> touchingCollider;
    [System.NonSerialized] public PlayerAbilities playerAbilities;
    [System.NonSerialized] public AttackProfiles attackProfile;
    [SerializeField] ParticleSystem fireVfx;
    [SerializeField] ParticleSystem electricityVfx;
    [SerializeField] ParticleSystem electricTrail;
    [SerializeField] Bolts bolts;
    GameManager _gm;
    GameManager gm
    {
        get
        {
            if(_gm == null)
            {
                _gm = GlobalEvents.instance.GetComponent<GameManager>();
            }
            return _gm;
        }
    }
    float startRadius = 2;
    int numberOfBoxes = 50;
    Animator animator;
    
    public bool blockable { get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        colliderScript = GetComponentInParent<TouchingCollider>();
        animator = GetComponent<Animator>();
        bolts.BoltsAway();
        switch (attackProfile.element)
        {
            case WeaponElement.FIRE:
                animator.SetInteger("Element", 1);
                fireVfx.Play();
                break;
            case WeaponElement.ELECTRICITY:
                animator.SetInteger("Element", 2);
                electricityVfx.Play();
                electricTrail.Play();
                break;
        }
    }

    public void Landing()
    {
        landingVFX.Play();
        RuntimeManager.PlayOneShot(attackProfile.noHitSoundEvent, attackProfile.soundNoHitVolume, transform.position);
        GlobalEvents.instance.ScreenShake(attackProfile.screenShakeNoHit);
        touchingCollider = colliderScript.GetTouchingObjects();
        int damage = playerAbilities.DetermineAttackDamage(attackProfile);
        //int poiseDamage = Mathf.RoundToInt(playerData.PhysicalDamage() * axeSpecial.damageMultiplier);
        foreach(Collider collider in touchingCollider)
        {
            if(collider != null)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyScript enemyScript = collider.gameObject.GetComponent<EnemyScript>();
                    playerAbilities.DamageEnemy(enemyScript, damage, attackProfile);
                }
                else if (collider.gameObject.CompareTag("Player") && false)
                {
                    PlayerScript playerScript = collider.gameObject.GetComponent<PlayerScript>();
                    playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
                    //playerScript.LosePoise(poiseDamage);
                    playerScript.StartStagger(attackProfile.staggerDuration);
                }
            }
        }
    }

    public void Ripple()
    {
        RuntimeManager.PlayOneShot(lanternSpecialRipple.noHitSoundEvent, lanternSpecialRipple.soundNoHitVolume, transform.position);
        float rotateAngle = 360 / numberOfBoxes;
        for (int box = 0; box < numberOfBoxes; box++)
        {
            Vector3 direction = Utils.RotateDirection(Vector3.right, rotateAngle * box);
            Vector3 startPos = transform.position + direction.normalized * startRadius;
            PlayerProjectileStraight.Instantiate(ripplePrefab, startPos, direction, lanternSpecialRipple, playerAbilities);
        }
    }

    public void Zap()
    {
        int attackDamage = playerAbilities.DetermineAttackDamage(zapProfile);
        RuntimeManager.PlayOneShot(zapProfile.noHitSoundEvent, zapProfile.soundNoHitVolume);
        GlobalEvents.instance.ScreenShake(zapProfile.screenShakeNoHit);
        List<Vector3> targets = Utils.HitClosestXEnemies(
            gm.enemies,
            transform.position,
            zapProfile.attackRange,
            zapProfile.boltNum,
            enemy => playerAbilities.DamageEnemy(enemy, attackDamage, attackProfile)
        );
        bolts.BoltsAoeAttackVfx(targets, fairySprite.transform.position);
    }

    public void SelfDestruct()
    {
        lanternFairy.Return(fairySprite.position);
        Destroy(gameObject);
    }
}
