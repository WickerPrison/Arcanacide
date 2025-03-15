using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireballAnimations : MonoBehaviour
{
    public float fireballCharge;
    [SerializeField] AttackProfiles fireballProfile;
    [SerializeField] AttackProfiles fireWaveProfile;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] GameObject fireWavePrefab;
    PlayerMovement playerMovement;
    PlayerFireball fireball;
    PlayerFireWave fireWave;
    PlayerScript playerScript;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerScript = playerMovement.gameObject.GetComponent<PlayerScript>();
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
        fireWave = Instantiate(fireWavePrefab).GetComponent<PlayerFireWave>();
        Vector3 direction = Vector3.Normalize(playerMovement.attackPoint.position - playerMovement.transform.position);
        fireWave.transform.position = playerMovement.transform.position + direction * 1.5f;
        fireWave.transform.LookAt(fireWave.transform.position + direction);
    }

    public void LaunchFireWave()
    {
        playerScript.LoseStamina(fireWaveProfile.staminaCost);
        fireWave.LaunchFireWave();
        fireWave = null;
    }
}
