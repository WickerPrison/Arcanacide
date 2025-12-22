using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    REGULAR, HEAVY, SPECIAL, DEFLECT
}

public enum HitboxType
{
    NONE, ARC, CIRCLE, AOE_ZAP
}

[CreateAssetMenu]
public class AttackProfiles : ScriptableObject
{
    public HitboxType hitbox;
    public AttackType attackType;
    public WeaponElement element;

    public int smearSpeed;
    public float stepWithAttack;
    public int halfConeAngle;
    public float attackRange;
    public float damageMultiplier;
    public float magicDamageMultiplier;
    public float poiseDamageMultiplier;
    public float staminaCost;
    public float chargeDamage;
    public float fullChargeDamage;
    public float maxChargeTime;
    public float manaCost;
    public float specialValue;
    public bool impactVFX = true;
    public bool blockable;

    public int boltNum;

    public int electricChargeBuildup;
    public float partialChargeBuildup;

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
    public List<Vector3> secondClawRot;
}
