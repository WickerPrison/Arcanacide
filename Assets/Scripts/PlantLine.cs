using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLine : MonoBehaviour
{
    [System.NonSerialized] public EnemyScript enemyOfOrigin;
    int damage = 10;
    float poiseDamage = 10;
    LayerMask layerMask;
    LayerMask playerMask;
    ChargeIndicator indicator;
    ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Default");
        playerMask = LayerMask.GetMask("Player");
        indicator = GetComponentInChildren<ChargeIndicator>();
        particles = GetComponentInChildren<ParticleSystem>();
        LayIndicator();
        StartCoroutine(DealDamage());
    }

    void LayIndicator()
    {
        float randomAngle = Random.Range(0f, 180f);
        Vector3 randomDirection = RotateDirection(Vector3.right, randomAngle);
        RaycastHit forwardHit;
        Physics.Raycast(transform.position, randomDirection, out forwardHit, 100, layerMask, QueryTriggerInteraction.Ignore);
        RaycastHit backHit;
        Physics.Raycast(transform.position, -randomDirection, out backHit, 100, layerMask, QueryTriggerInteraction.Ignore);

        transform.position = Vector3.zero;
        indicator.initialPosition = forwardHit.point;
        indicator.initialNormal = forwardHit.normal;
        indicator.finalPosition = backHit.point;
        indicator.finalNormal = backHit.normal;
    }

    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(2);
        PlayerDetection();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    void PlayerDetection()
    {
        particles.Play();
        Vector3 centerPoint = (indicator.initialPosition + indicator.finalPosition) / 2;
        Quaternion direction = Quaternion.LookRotation(indicator.finalPosition - indicator.initialPosition);
        Vector3 halfExtents = new Vector3(indicator.indicatorWidth / 2, indicator.indicatorWidth / 2, Vector3.Distance(indicator.finalPosition, indicator.initialPosition) / 2);
        Collider[] hitColliders = Physics.OverlapBox(centerPoint, halfExtents, direction, playerMask, QueryTriggerInteraction.Ignore);
        if (hitColliders.Length > 0)
        {
            HitPlayer(hitColliders[0].GetComponent<PlayerScript>());
        }
    }

    void HitPlayer(PlayerScript playerScript)
    {
        playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, enemyOfOrigin);
        playerScript.LosePoise(poiseDamage);
        //AudioSource.PlayClipAtPoint(playerImpactSFX, transform.position, impactSFXvolume);
    }

    Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.z;
        newDirection.z = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.z;
        return newDirection;
    }
}
