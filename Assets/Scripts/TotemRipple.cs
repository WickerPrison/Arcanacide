using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TotemRipple : WaveBox
{
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemyScript = other.gameObject.GetComponent<EnemyScript>();
            enemyScript.LoseHealth(damage, poiseDamage);
            PlaySound(playerImpactSFX, playerImpactVolume);
            Destroy(gameObject);
        }
        else
        {
            PlaySound(impactSFX, impactVolume);
            Destroy(gameObject);
        }
    }


}
