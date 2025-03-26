using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

public class FireRain : PlayerProjectile, IKillChildren
{
    [System.NonSerialized] public Vector3 origin;
    [System.NonSerialized] public float maxDelay;
    [System.NonSerialized] public bool hasNavmesh = true;
    [SerializeField] ParticleSystem vfx;
    [SerializeField] KilledByParent indicatorCircle;
    public event EventHandler onKillChildren;
    Vector3 destination;
    Vector3 direction;
    bool start = false;
    Collider hitBox;

    private void Awake()
    {
        indicatorCircle.transform.SetParent(null);
        indicatorCircle.parent = this;
    }

    private void Start()
    {
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

    public override void FixedUpdate()
    {
        if (!start) return;
        transform.LookAt(destination);
        transform.position += Time.fixedDeltaTime * speed * direction;

        if (transform.position.y < 0)
        {
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
