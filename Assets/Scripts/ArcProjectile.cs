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

    float totDist;
    float a;
    float b;
    float c;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerCollider = player.gameObject.GetComponent<Collider>();


        startPoint = transform.position;
        direction = new Vector3(endPoint.x, 0, endPoint.z) - new Vector3(startPoint.x, 0, startPoint.z);

        totDist = Vector2.Distance(new Vector2(startPoint.x, startPoint.z), new Vector2(endPoint.x, endPoint.z));

        speed = totDist / timeToHit;

        float w2 = Mathf.Pow(thirdPointX, 2);
        a = -((arcHeight + startPoint.y * w2) / (thirdPointX - w2));
        b = (arcHeight + startPoint.y * w2)/(thirdPointX - w2) - startPoint.y;
        c = startPoint.y;

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
        Vector3 nextPosition = currentPosition + direction.normalized * Time.fixedDeltaTime * speed;
        float xVal = InverseLerpSetY0(startPoint, endPoint, nextPosition);
        float nextHeight = a * Mathf.Pow(xVal, 2) + b * xVal + c;
        return new Vector3(nextPosition.x, nextHeight, nextPosition.z);
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
            RuntimeManager.PlayOneShot(impactSound, impactVolume);
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

    float InverseLerpSetY0(Vector3 startPos, Vector3 endPos, Vector3 currentPos)
    {
        endPos = new Vector3(endPos.x, 0, endPos.z);
        startPos = new Vector3(startPos.x, 0, startPos.z);
        currentPos = new Vector3(currentPos.x,0, currentPos.z);
        Vector3 diff =  endPos - startPos;
        Vector3 currentDiff = currentPos - startPos;
        return Vector3.Dot(currentDiff, diff)/Vector3.Dot(diff, diff);
    }
}
