using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFireballAnimations : MonoBehaviour
{
    public float fireballCharge;
    [SerializeField] AttackProfiles fireWaveProfile;
    [SerializeField] AttackProfiles electricArtilleryProfile;
    [SerializeField] AttackProfiles fireWaveTrailProfile;
    [SerializeField] AttackProfiles fireCircleTrailProfile;
    [SerializeField] GameObject fireWavePrefab;
    [SerializeField] GameObject electricArtilleryPrefab;
    [SerializeField] GameObject fireCirclePrefab;
    [SerializeField] PlayerData playerData;
    PlayerMovement playerMovement;
    PlayerFireball fireball;
    PlayerFireWave fireWave;
    PlayerScript playerScript;
    PlayerAbilities playerAbilities;
    PlayerTrailManager trailManager;
    OrbitFlames orbitFlames;
    public event EventHandler onLaunchFireWave;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerScript = playerMovement.gameObject.GetComponent<PlayerScript>();
        playerAbilities = playerMovement.gameObject.GetComponent<PlayerAbilities>();
        trailManager = playerMovement.gameObject.GetComponent<PlayerTrailManager>();
        orbitFlames = playerMovement.GetComponent<OrbitFlames>();
    }

    public void SpawnFireball(AttackHit attackHit)
    {
        fireball = PlayerProjectile.Instantiate(attackHit.GetPrefab(playerData.equippedElements[1]), attackHit.GetProfile(playerData.equippedElements[1]), playerAbilities) as PlayerFireball;
        fireball.fireballAnimations = this;
        fireball.playerMovement = playerMovement;
    }

    public void LaunchFireball(AttackHit attackHit)
    {
        playerScript.LoseStamina(attackHit.GetProfile(playerData.equippedElements[1]).staminaCost);
        fireball.LaunchFireball();
        fireball = null;
    }

    public void SpawnFireWave()
    {
        Vector3 direction = Vector3.Normalize(playerMovement.attackPoint.position - playerMovement.transform.position);
        switch (playerData.equippedElements[1])
        {
            case WeaponElement.FIRE:
                Vector3 spawnPosition = playerMovement.transform.position + direction * 1.5f;
                fireWave = PlayerFireWave.Instantiate(fireWavePrefab, spawnPosition, direction, trailManager, fireWaveProfile, fireWaveTrailProfile);
                break;
            case WeaponElement.ELECTRICITY:
                Vector3 spawnOrigin = playerMovement.transform.position - direction * 1.5f;
                for(int i = 0; i < 5; i++)
                {
                    ElectricArtillery.Instantiate(electricArtilleryPrefab, spawnOrigin, direction, this, electricArtilleryProfile, playerAbilities);
                }
                break;
        }

    }

    public void LaunchFireWave()
    {
        switch (playerData.equippedElements[1])
        {
            case WeaponElement.FIRE:
                playerScript.LoseStamina(fireWaveProfile.staminaCost);
                fireWave.LaunchFireWave();
                fireWave = null;
                break;
            case WeaponElement.ELECTRICITY:
                onLaunchFireWave?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    public void SpawnFireCircle()
    {
        if (playerData.equippedElements[0] != WeaponElement.FIRE) return;
        PlayerFireCircle.Instantiate(fireCirclePrefab, playerScript.transform.position, trailManager, fireCircleTrailProfile);
    }

    public void SpawnOrbitFlames()
    {
        if (playerData.equippedElements[0] != WeaponElement.FIRE) return;
        orbitFlames.InitialSpawn();
    }
}
