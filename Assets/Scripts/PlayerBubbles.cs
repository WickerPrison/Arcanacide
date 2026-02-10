using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.AI;

public class PlayerBubbles : MonoBehaviour
{
    Vector3 direction;
    ChargeIndicator indicator;
    LayerMask mask;
    [SerializeField] LayerMask enemyMask;
    float chargeIndicatorWidth = 1.2f;
    int damage = 50;
    float poiseDamage = 50;
    float extensionLength = 0.11f;
    [SerializeField] ParticleSystem particleSys;
    [SerializeField] float randomRange = 12f;
    bool instantiatedCorrectly = false;
    Vector3 center;
    [SerializeField] AttackProfiles attackProfile;
    PlayerAbilities playerAbilities;

    public static PlayerBubbles Instantiate(GameObject prefab, Vector3 center, PlayerAbilities playerAbilities)
    {
        PlayerBubbles bubbles = Instantiate(prefab).GetComponent<PlayerBubbles>();
        bubbles.center = center;
        bubbles.playerAbilities = playerAbilities;
        bubbles.instantiatedCorrectly = true;
        return bubbles;
    }

    private void Awake()
    {
        mask = LayerMask.GetMask("Default");

        float xPos = Random.Range(-randomRange, randomRange);
        float zPos = Random.Range(-randomRange, randomRange);
        NavMeshHit hit;
        if(NavMesh.SamplePosition(center + new Vector3(xPos, 0, zPos), out hit, 3, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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
        indicator.ReStart();
        indicator.Hide();

        SetupParticleSystem();

        StartCoroutine(BeamTimer());
    }

    private void Start()
    {
        if (!instantiatedCorrectly) Utils.IncorrectInitialization("PlayerBubbles");
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

        EnemyDetection();
        FmodUtils.PlayOneShot(attackProfile.noHitSoundEvent, attackProfile.soundNoHitVolume);
        indicator.Hide();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    void EnemyDetection()
    {
        Vector3 centerPoint = (indicator.initialPosition + indicator.finalPosition) / 2;
        Quaternion direction = Quaternion.LookRotation(indicator.finalPosition - indicator.initialPosition);
        Vector3 halfExtents = new Vector3(indicator.indicatorWidth / 2, indicator.indicatorWidth / 2, Vector3.Distance(indicator.finalPosition, indicator.initialPosition) / 2);
        Collider[] hitColliders = Physics.OverlapBox(centerPoint, halfExtents, direction, enemyMask, QueryTriggerInteraction.Ignore);
        foreach(Collider collider in hitColliders)
        {
            HitEnemy(collider.GetComponent<EnemyScript>());
        }
    }

    void HitEnemy(EnemyScript enemyScript)
    {
        int damage = playerAbilities.DetermineAttackDamage(attackProfile);
        playerAbilities.DamageEnemy(enemyScript, damage, attackProfile);
        GlobalEvents.instance.ScreenShake(attackProfile.screenShakeOnHit);
    }
}
