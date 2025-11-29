using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitFlames : MonoBehaviour
{
    [SerializeField] GameObject orbitFlamePrefab;
    [SerializeField] AttackProfiles attackProfile;
    OrbitFlame[] orbitFlames = new OrbitFlame[3];
    PlayerScript playerScript;
    float angle;
    [SerializeField] float speed;
    bool active = false;

    private void Start()
    {
        playerScript = GetComponent<PlayerScript>();
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
}
