using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackProfiles : ScriptableObject
{
    public int smearSpeed;
    public int halfConeAngle;
    public float attackArcRadius;
    public int damageMultiplier;
    public int poiseDamageMultiplier;
}
