using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireball : PlayerProjectile
{
    [SerializeField] float minSize;
    [SerializeField] float maxSize;
    [SerializeField] ParticleSystem vfx;
    [System.NonSerialized] public PlayerFireballAnimations fireballAnimations;
    enum FireballState
    {
        OFF, CHARGING, ACTIVE
    }
    FireballState state;

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
                transform.position = playerMovement.attackPoint.position;
                break;
            case FireballState.ACTIVE:
                base.FixedUpdate();
                break;
        }
    }
}
