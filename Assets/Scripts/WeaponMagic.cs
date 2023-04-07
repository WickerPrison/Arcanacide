using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMagic : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> weaponMagics = new List<ParticleSystem>();
    [SerializeField] List<ParticleSystem> offhandWeaponMagics = new List<ParticleSystem>();
    [SerializeField] PlayerData playerData;
    WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponentInParent<WeaponManager>();
        weaponManager.OnStartWeaponMagic += WeaponManager_OnStartWeaponMagic;
        weaponManager.OnStopWeaponMagic += WeaponManager_OnStopWeaponMagic;
    }

    private void WeaponManager_OnStopWeaponMagic(object sender, System.EventArgs e)
    {
        weaponMagics[playerData.currentWeapon].Stop();
        if (offhandWeaponMagics[playerData.currentWeapon] != null)
        {
            offhandWeaponMagics[playerData.currentWeapon].Stop();
        }
    }

    private void WeaponManager_OnStartWeaponMagic(object sender, System.EventArgs e)
    {
        weaponMagics[playerData.currentWeapon].Play();
        if (offhandWeaponMagics[playerData.currentWeapon] != null)
        {
            offhandWeaponMagics[playerData.currentWeapon].Play();
        }
    }
}
