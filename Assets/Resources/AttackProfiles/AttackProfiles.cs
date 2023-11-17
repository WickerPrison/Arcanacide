using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    REGULAR, HEAVY, SPECIAL, DEFLECT
}

[CreateAssetMenu]
public class AttackProfiles : ScriptableObject
{
    public string hitboxType;
    public AttackType attackType;

    public int smearSpeed;
    public float stepWithAttack;
    public int halfConeAngle;
    public float attackRange;
    public float damageMultiplier;
    public float magicDamageMultiplier;
    public float poiseDamageMultiplier;
    public float staminaCost;
    public float manaCost;

    public float bonusEffectDuration;
    public float durationDOT;
    public float staggerDuration;

    public Vector2 screenShakeOnHit;
    public Vector2 screenShakeNoHit;

    public EventReference soundOnHitEvent;
    public float soundOnHitVolume;
    public EventReference noHitSoundEvent;
    public float soundNoHitVolume;

    public List<Vector3> smearPositions;
    public List<Vector3> smearRotations;
    public List<Vector3> smearScales;
}
