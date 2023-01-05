using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackProfiles : ScriptableObject
{
    public int smearSpeed;
    public int halfConeAngle;
    public float attackRange;
    public int damageMultiplier;
    public int magicDamageMultiplier;
    public int poiseDamageMultiplier;
    public float staminaCost;
}
