using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashProjectile : MonoBehaviour
{
    Vector3 direction;
    PlayerAbilities playerAbilities;
    [SerializeField] AttackProfiles attackProfile;
    [SerializeField] float speed;
    [SerializeField] float lifetime;
    bool instantiatedCorrectly = false;

    public static SlashProjectile Instantiate(GameObject prefab, Vector3 spawnPosition, Vector3 direction, PlayerAbilities playerAbilities)
    {
        SlashProjectile slashProjectile = GameObject.Instantiate(prefab).GetComponent<SlashProjectile>();
        slashProjectile.transform.position = spawnPosition;
        slashProjectile.direction = direction;
        slashProjectile.playerAbilities = playerAbilities;
        slashProjectile.instantiatedCorrectly = true;
        return slashProjectile;
    }

    private void Start()
    {
        if (!instantiatedCorrectly)
        {
            Utils.IncorrectInitialization("PlayerFireWave");
        }
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void FixedUpdate()
    {
        transform.position += Time.fixedDeltaTime * speed * direction.normalized;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            int damage = playerAbilities.DetermineAttackDamage(attackProfile);
            playerAbilities.DamageEnemy(enemyScript, damage, attackProfile);
        }
    }
}
