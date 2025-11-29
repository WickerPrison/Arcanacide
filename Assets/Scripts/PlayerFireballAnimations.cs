using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireballAnimations : MonoBehaviour
{
    public float fireballCharge;
    [SerializeField] AttackProfiles fireballProfile;
    [SerializeField] AttackProfiles fireWaveProfile;
    [SerializeField] AttackProfiles fireWaveTrailProfile;
    [SerializeField] AttackProfiles fireCircleTrailProfile;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] GameObject fireWavePrefab;
    [SerializeField] GameObject fireCirclePrefab;
    [SerializeField] PlayerData playerData;
    PlayerMovement playerMovement;
    PlayerFireball fireball;
    PlayerFireWave fireWave;
    PlayerScript playerScript;
    PlayerTrailManager trailManager;
    OrbitFlames orbitFlames;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerScript = playerMovement.gameObject.GetComponent<PlayerScript>();
        trailManager = playerMovement.gameObject.GetComponent<PlayerTrailManager>();
        orbitFlames = playerMovement.GetComponent<OrbitFlames>();
    }

    public void SpawnFireball()
    {
        fireball = Instantiate(fireballPrefab).GetComponent<PlayerFireball>();
        fireball.fireballAnimations = this;
        fireball.playerMovement = playerMovement;
        fireball.attackProfile = fireballProfile;
    }

    public void LaunchFireball()
    {
        playerScript.LoseStamina(fireballProfile.staminaCost);
        fireball.LaunchFireball();
        fireball = null;
    }

    public void SpawnFireWave()
    {
        Vector3 direction = Vector3.Normalize(playerMovement.attackPoint.position - playerMovement.transform.position);
        Vector3 spawnPosition = playerMovement.transform.position + direction * 1.5f;
        fireWave = PlayerFireWave.Instantiate(fireWavePrefab, spawnPosition, direction, trailManager, fireWaveProfile, fireWaveTrailProfile);
    }

    public void LaunchFireWave()
    {
        playerScript.LoseStamina(fireWaveProfile.staminaCost);
        fireWave.LaunchFireWave();
        fireWave = null;
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
