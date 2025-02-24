using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireballAnimations : MonoBehaviour
{
    public float fireballCharge;
    [SerializeField] AttackProfiles fireballProfile;
    [SerializeField] GameObject fireballPrefab;
    PlayerMovement playerMovement;
    PlayerFireball fireball;
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
        playerScript.LoseStamina(fireballProfile.staminaCost);
    }

    public void LaunchFireball()
    {
        fireball.LaunchFireball();
        fireball = null;
    }
}
