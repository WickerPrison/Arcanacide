using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmear : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    ParticleSystem smear;
    ParticleSystem.ShapeModule smearShape;
    // front right = 0, front left = 1, back left = 2, back right = 3

    private void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        smear = GetComponent<ParticleSystem>();
        smearShape = smear.shape;
        smear.Simulate(1);
    }

    public void particleSmear(AttackProfiles attackProfile)
    {
        if(attackProfile.smearSpeed == 0)
        {
            return;
        }

        smear.Clear();
        SmearDirection(attackProfile);
        ParticleSystem.ShapeModule smearShapeFront = smear.shape;
        smearShapeFront.arcSpeed = attackProfile.smearSpeed;
        smear.Simulate(0);
    }

    void SmearDirection(AttackProfiles attackProfile)
    {
        smear.transform.localScale = attackProfile.smearScales[playerAnimation.facingDirection];
        smear.transform.localPosition = attackProfile.smearPositions[playerAnimation.facingDirection];
        Vector3 rotation = attackProfile.smearRotations[playerAnimation.facingDirection];
        smear.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    public void SpeedMultiplier(float multiplier)
    {
        smearShape.arcSpeedMultiplier = multiplier;
    }

    private void FixedUpdate()
    {
        smear.Simulate(Time.fixedDeltaTime, true, false);
    }
}
