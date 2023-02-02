using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackProfiles : ScriptableObject
{
    public string hitboxType;
    public bool heavyAttack;

    public int smearSpeed;
    public int halfConeAngle;
    public float attackRange;
    public float damageMultiplier;
    public float magicDamageMultiplier;
    public float poiseDamageMultiplier;
    public float staminaCost;

    public float durationDOT;
    public float staggerDuration;

    public Vector2 screenShakeOnHit;
    public Vector2 screenShakeNoHit;

    public AudioClip soundOnHit;
    public float soundOnHitVolume;
    public AudioClip soundNoHit;
    public float soundNoHitVolume;
}
