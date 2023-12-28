using FMODUnity;
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
    float chargeIndicatorWidth = 1.2f;
    int damage = 50;
    float poiseDamage = 50;
    float extensionLength = 0.11f;
    [SerializeField] ParticleSystem particleSys;
    [SerializeField] float randomRange = 12f;
    [SerializeField] EventReference sfx;

    private void Awake()
    {
        mask = LayerMask.GetMask("Default");
        playerMask = LayerMask.GetMask("Player");

        float xPos = Random.Range(-randomRange, randomRange);
        float zPos = Random.Range(-randomRange, randomRange);
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

        SetupParticleSystem();

        StartCoroutine(BeamTimer());
    }


    void SetupParticleSystem()
    {
        ParticleSystem.ShapeModule shapeModule = particleSys.shape;
        shapeModule.scale = new Vector3(chargeIndicatorWidth * 0.8f, Vector3.Distance(indicator.initialPosition, indicator.finalPosition), 0);

        particleSys.transform.position = (indicator.initialPosition + indicator.finalPosition) / 2;

        shapeModule.rotation = Quaternion.LookRotation(particleSys.transform.position - indicator.initialPosition).eulerAngles;
        shapeModule.rotation = new Vector3(-90, shapeModule.rotation.y, shapeModule.rotation.z);
        ParticleSystem.EmissionModule emissionModule = particleSys.emission;
        Burst burst = emissionModule.GetBurst(0);
        burst.count = Mathf.RoundToInt(5 * shapeModule.scale.y);
        emissionModule.SetBurst(0, burst);
    }

    IEnumerator BeamTimer()
    {
        yield return new WaitForSeconds(2);
        PlayerDetection();
        RuntimeManager.PlayOneShot(sfx);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
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
        if(playerScript.gameObject.layer == 3)
        {
            playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
            playerScript.LosePoise(poiseDamage);
        }
    }
}
