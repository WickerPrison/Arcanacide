using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashProjectile : MonoBehaviour
{
    [System.NonSerialized] public Vector3 direction;
    [System.NonSerialized] public PlayerAbilities playerAbilities;
    [SerializeField] AttackProfiles attackProfile;
    [SerializeField] float speed;
    [SerializeField] float lifetime;

    private void Start()
    {
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
