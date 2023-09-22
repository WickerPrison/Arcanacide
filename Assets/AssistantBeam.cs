using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class AssistantBeam : MonoBehaviour
{
    Vector3 direction;
    ChargeIndicator indicator;
    LayerMask mask;
    LayerMask playerMask;
    float chargeIndicatorWidth = 0.5f;
    int damage = 50;
    float poiseDamage = 50;
    float extensionLength = 0.11f;

    private void Awake()
    {
        mask = LayerMask.GetMask("Default");
        playerMask = LayerMask.GetMask("Player");

        float xPos = Random.Range(-15, 15);
        float zPos = Random.Range(-15, 15);
        transform.position = new Vector3(xPos, 0, zPos);

        float xDir = Random.Range(-1f, 1f);
        float zDir = Random.Range(-1f, 1f);
        direction = new Vector3(xDir, 0, zDir).normalized;

        RaycastHit hit1;
        Physics.Raycast(transform.position, direction, out hit1, 100, mask);
        RaycastHit hit2;
        Physics.Raycast(transform.position, -direction, out hit2, 100, mask);

        indicator = GetComponentInChildren<ChargeIndicator>();
        transform.position = Vector3.zero;
        indicator.initialPosition = hit1.point + direction * extensionLength;
        indicator.initialNormal = hit1.normal;
        indicator.finalPosition = hit2.point - direction * extensionLength;
        indicator.finalNormal = hit2.normal;
        indicator.indicatorWidth = chargeIndicatorWidth;
    }

    void PlayerDetection()
    {
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
        playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
        playerScript.LosePoise(poiseDamage);
    }
}
