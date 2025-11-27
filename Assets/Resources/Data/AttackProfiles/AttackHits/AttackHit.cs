using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerWeapon
{
    SWORD, LANTERN, KNIFE, CLAWS
}

[CreateAssetMenu]
public class AttackHit : ScriptableObject
{
    public PlayerWeapon weaponCategory;
}
