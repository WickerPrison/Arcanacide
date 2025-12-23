using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerWeapon
{
    SWORD, LANTERN, KNIFE, CLAWS
}

public enum WeaponElement
{
    DEFAULT, FIRE, ELECTRICITY, ICE, CHAOS
}

[CreateAssetMenu]
public class AttackHit : ScriptableObject
{
    public PlayerWeapon weaponCategory;
    public AttackProfiles defaultProfile;
    public AttackProfiles fireProfile;
    public AttackProfiles electricityProfile;
    public AttackProfiles iceProfile;
    public AttackProfiles chaosProfile;
    [SerializeField] GameObject[] prefabs;

    public AttackProfiles GetProfile(WeaponElement element)
    {
        switch (element)
        {
            case WeaponElement.DEFAULT: return defaultProfile;
            case WeaponElement.FIRE: return fireProfile;
            case WeaponElement.ELECTRICITY: return electricityProfile;
            case WeaponElement.ICE: return iceProfile;
            case WeaponElement.CHAOS: return chaosProfile;
            default: return null;
        }
    }

    public GameObject GetPrefab(WeaponElement element)
    {
        switch (element)
        {
            case WeaponElement.DEFAULT: return prefabs[0];
            case WeaponElement.FIRE: return prefabs[1];
            case WeaponElement.ELECTRICITY: return prefabs[2];
            case WeaponElement.ICE: return prefabs[3];
            case WeaponElement.CHAOS: return prefabs[4];
            default: return null;
        }
    }
}
