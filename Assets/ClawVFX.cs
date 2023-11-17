using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawVFX : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform attackPoint;
    ParticleSystem[] particleSystems;
    PlayerAnimation playerAnimation;

    private void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        playerAnimation = GetComponentInParent<PlayerAnimation>();
    }

    public void StartVFX(AttackProfiles attackProfile)
    {
        foreach (ParticleSystem swipe in particleSystems)
        {
            ParticleSystem.MainModule main = swipe.main;
            main.startRotationY = (attackPoint.rotation.eulerAngles.y + attackProfile.smearRotations[playerAnimation.facingDirection].y) * Mathf.Deg2Rad;
            main.startRotationX = attackProfile.smearRotations[playerAnimation.facingDirection].x * Mathf.Deg2Rad;
            

            swipe.Play();
        }
    }
}
