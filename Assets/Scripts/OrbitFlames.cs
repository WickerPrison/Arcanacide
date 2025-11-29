using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitFlames : MonoBehaviour
{
    [SerializeField] GameObject orbitFlamePrefab;
    [SerializeField] AttackProfiles attackProfile;
    [SerializeField] PlayerData playerData;
    OrbitFlame[] orbitFlames = new OrbitFlame[3];
    PlayerScript playerScript;
    PlayerAbilities playerAbilities;
    float angle;
    [SerializeField] float speed;
    bool active = false;
    int damageCounter;
    int _rechargeDamage;
    int rechargeDamage
    {
        get
        {
            if(_rechargeDamage == 0)
            {
                _rechargeDamage = playerAbilities.DetermineAttackDamage(attackProfile);
            }
            return _rechargeDamage;
        }
    }

    private void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        playerAbilities = GetComponent<PlayerAbilities>();
    }

    private void FixedUpdate()
    {
        if (!active) return;
        angle += Time.fixedDeltaTime * speed;
        PositionOrbs();
    }

    void PositionOrbs()
    {
        foreach(OrbitFlame orbitFlame in orbitFlames)
        {
            if(orbitFlame == null) continue;
            orbitFlame.PositionFlame(angle);
        }
    }

    public void RefillSlot()
    {
        for (int i = 0; i < orbitFlames.Length; i++)
        {
            if (orbitFlames[i] == null)
            {
                SpawnOrbitFlame(i);
                break;
            }
        }
    } 

    public void SpawnOrbitFlame(int i)
    {
        orbitFlames[i] = OrbitFlame.Instantiate(orbitFlamePrefab, i * 120, 2, playerScript, RemoveOrbitFlame, attackProfile);
    }

    public void InitialSpawn()
    {
        active = true;
        for (int i = 0; i < 3; ++i) 
        {
            SpawnOrbitFlame(i);
        }
    }

    void RemoveOrbitFlame(OrbitFlame orbitFlame)
    {
        int index = Array.IndexOf(orbitFlames, orbitFlame);
        orbitFlames[index] = null;
    }

    private void Global_onPlayerDealDamage(object sender, int damage)
    {
        if (playerData.swordSpecialTimer <= 0 || playerData.currentWeapon != 0 || playerData.equippedElements[0] != WeaponElement.FIRE) return;
        damageCounter += damage;
        if(damageCounter > rechargeDamage)
        {
            damageCounter = 0;
            RefillSlot();
        }
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onPlayerDealDamage += Global_onPlayerDealDamage;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onPlayerDealDamage -= Global_onPlayerDealDamage;
    }
}
