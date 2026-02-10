using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArcProjectile : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject indicatorCirclePrefab;
    TouchingCollider touchingCircle;
    [SerializeField] float explosionRadius;
    public Vector3 direction;
    public float timeToHit;
    float speed;
    public int spellDamage;
    public int poiseDamage;
    PlayerScript player;
    Collider playerCollider;
    public Vector3 endPoint;
    Vector3 startPoint;
    [SerializeField] float arcHeight;
    [SerializeField] float thirdPointX;
    public EventReference impactSound;
    public float impactVolume;
    [SerializeField] float staggerDuration;
    [System.NonSerialized] public EnemyScript enemyOfOrigin;
    ArcUtils.ArcData arcData;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerCollider = player.gameObject.GetComponent<Collider>();

        arcData = ArcUtils.CreateArcData(transform.position, endPoint, timeToHit, arcHeight, thirdPointX);

        SpawnIndicator();
    }

    public virtual void FixedUpdate()
    {
        transform.position = GetNextPosition(transform.position);

        if (transform.position.y <= 0.3f)
        {
            Explosion();
        }
    }

    public Vector3 GetNextPosition(Vector3 currentPosition)
    {
        return ArcUtils.GetNextArcPosition(currentPosition, arcData);
    }

    public virtual void SpawnIndicator()
    {
        IndicatorCircle indicatorCircle = Instantiate(indicatorCirclePrefab).GetComponent<IndicatorCircle>();
        indicatorCircle.transform.position = new Vector3(endPoint.x, 0, endPoint.z);
        indicatorCircle.finalScale = explosionRadius;
        indicatorCircle.startScale = explosionRadius;
        indicatorCircle.deathTime = timeToHit + 0.1f;
        touchingCircle = indicatorCircle.gameObject.GetComponentInChildren<TouchingCollider>();
    }

    public virtual void Explosion()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = new Vector3(endPoint.x, .3f, endPoint.z);
        DestroyProjectile();

        List<Collider> objects = touchingCircle.GetTouchingObjects();
        if (objects.Contains(playerCollider) && player.gameObject.layer == 3)
        {
            FmodUtils.PlayOneShot(impactSound, impactVolume);
            player.StartStagger(staggerDuration);
            player.LoseHealth(spellDamage, EnemyAttackType.PROJECTILE, enemyOfOrigin);
            player.LosePoise(poiseDamage);
        }
        else if(objects.Contains(playerCollider) && player.gameObject.layer == 8)
        {
            player.GetComponent<PlayerScript>().PerfectDodge(EnemyAttackType.PROJECTILE);
        }

    }

    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
