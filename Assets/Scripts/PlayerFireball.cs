using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : PlayerProjectile
{
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] ParticleSystem vfx;
    [System.NonSerialized] public PlayerFireballAnimations fireballAnimations;
    Collider hitCollider;
    GameObject dummyTarget;
    enum FireballState
    {
        OFF, CHARGING, ACTIVE
    }
    FireballState state;
    Vector3 vertOffset = new Vector3(0, 2f, 0);

    public override void Start()
    {
        base.Start();
        state = FireballState.CHARGING;
        hitCollider = GetComponent<Collider>();
        hitCollider.enabled = false;
        transform.position = GetChargingPosition();
        vfx.Play();
    }

    public override void Update()
    {
        switch (state)
        {
            case FireballState.CHARGING:
                vfx.transform.localScale = Mathf.Lerp(minSize, maxSize, fireballAnimations.fireballCharge) * Vector3.one;
                break;
            case FireballState.ACTIVE:
                base.Update();
                break;
        }
    }

    public override void FixedUpdate()
    {
        switch (state)
        {
            case FireballState.CHARGING:
                transform.position = GetChargingPosition();
                break;
            case FireballState.ACTIVE:
                base.FixedUpdate();
                break;
        }
    }

    public void LaunchFireball()
    {
        state = FireballState.ACTIVE;
        target = GetTarget();
        transform.LookAt(target.position);
        hitCollider.enabled = true;
    }

    Transform GetTarget()
    {
        Transform lockOnTarget = playerMovement.GetLockOnTarget();
        if (target != null) return lockOnTarget;
        dummyTarget = new GameObject("Dummy Target");
        Vector3 lookDirection = Vector3.Normalize(playerMovement.attackPoint.position - playerMovement.transform.position);
        dummyTarget.transform.position = playerMovement.transform.position + lookDirection.normalized * 10;
        return dummyTarget.transform;
    }

    Vector3 GetChargingPosition()
    {
        Vector3 direction = Vector3.Normalize(playerMovement.attackPoint.position - playerMovement.transform.position);
        return playerMovement.transform.position + direction * 1.5f + vertOffset;
    }

    public override void KillProjectile()
    {
        if (dummyTarget != null) Destroy(dummyTarget);
        vfx.Stop();
        StartCoroutine(KillCoroutine());
    }

    IEnumerator KillCoroutine()
    {
        hitCollider.enabled = false;
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
