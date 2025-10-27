using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TotemRipple : WaveBox, IDamageEnemy
{
    public bool blockable { get; set; } = true;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            enemyScript.LoseHealth(damage, poiseDamage, this, () =>
            {
                PlaySound(playerImpactSFX, playerImpactVolume);
            });
            Destroy(gameObject);
        }
        else
        {
            PlaySound(impactSFX, impactVolume);
            Destroy(gameObject);
        }
    }


}
