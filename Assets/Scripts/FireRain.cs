using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

public class FireRain : PlayerProjectile, IKillChildren
{
    Vector3 origin;
    float maxDelay;
    bool hasNavmesh = true;
    [SerializeField] ParticleSystem vfx;
    [SerializeField] KilledByParent indicatorCircle;
    AttackProfiles trailProfile;
    [SerializeField] GameObject fireTrailPrefab;
    public event EventHandler onKillChildren;
    Vector3 destination;
    Vector3 direction;
    bool start = false;
    Collider hitBox;
    PlayerTrailManager trailManager;

    public static FireRain Instantiate(GameObject prefab, Vector3 origin, float maxDelay, AttackProfiles attackProfile, AttackProfiles trailProfile, PlayerTrailManager trailManager, bool hasNavmesh = true)
    {
        PlayerAbilities playerAbilities = trailManager.GetComponent<PlayerAbilities>();
        FireRain fireRain = PlayerProjectile.Instantiate(prefab, attackProfile, playerAbilities) as FireRain;
        fireRain.origin = origin;
        fireRain.maxDelay = maxDelay;
        fireRain.trailProfile = trailProfile;
        fireRain.trailManager = trailManager;
        fireRain.hasNavmesh = hasNavmesh;
        return fireRain;
    }

    private void Awake()
    {
        indicatorCircle.transform.SetParent(null);
        indicatorCircle.parent = this;
    }

    public override void Start()
    {
        base.Start();
        transform.position = origin;
        float randX = UnityEngine.Random.Range(-maxDelay, maxDelay);
        float randZ = UnityEngine.Random.Range(-maxDelay, maxDelay);
        destination = new Vector3(origin.x + randX, 0, origin.z + randZ);
        if (hasNavmesh)
        {
            FindTargetWithNavmesh();
        }
        else
        {
            FindTargetNoNavmesh();
        }
        hitBox = GetComponent<Collider>();
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 5f));
        hitBox.enabled = true;
        start = true;
        vfx.Play();
    }

    public override void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitEnemy(collision, false);
        }
        else
        {
            HitObject(collision);
        }
    }

    public override void FixedUpdate()
    {
        if (!start) return;
        transform.LookAt(destination);
        transform.position += Time.fixedDeltaTime * speed * direction;

        if (transform.position.y < 0)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, 0, transform.position.z);
            PathTrail.Instantiate(fireTrailPrefab, spawnPosition, trailManager, trailProfile);
            HitObject(GetComponent<Collider>());
        }
    }

    void FindTargetWithNavmesh()
    {
        NavMeshHit outHit;
        if (NavMesh.SamplePosition(destination, out outHit, 0.1f, NavMesh.AllAreas))
        {
            destination = outHit.position;
            direction = Vector3.Normalize(destination - transform.position);
            indicatorCircle.transform.position = destination;
        }
        else
        {
            KillProjectile();
        }
    }

    void FindTargetNoNavmesh()
    {
        Vector3 rayDirection = destination - origin;
        LayerMask layerMask = LayerMask.GetMask("Floor");
        if (Physics.Raycast(origin, rayDirection, 30f, layerMask))
        {
            direction = rayDirection.normalized;
            indicatorCircle.transform.position = destination;
        }
        else
        {
            KillProjectile();
        }
    }

    public override void KillProjectile()
    {
        vfx.Stop();
        start = false;
        onKillChildren?.Invoke(this, EventArgs.Empty);
        StartCoroutine(KillCoroutine());
    }

    IEnumerator KillCoroutine()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
