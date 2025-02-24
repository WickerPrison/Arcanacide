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
    Vector3 vertOffset = new Vector3(0, 1.5f, 0);

    private void Start()
    {
        state = FireballState.CHARGING;
        hitCollider = GetComponent<Collider>();
        hitCollider.enabled = false;
        transform.position = playerMovement.attackPoint.position + vertOffset;
        vfx.Play();
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
                transform.position = playerMovement.attackPoint.position + vertOffset;
                break;
            case FireballState.ACTIVE:
                base.FixedUpdate();
                break;
        }
    }

    private void OnDestroy()
    {
        if (dummyTarget != null) Destroy(dummyTarget);
    }
}
