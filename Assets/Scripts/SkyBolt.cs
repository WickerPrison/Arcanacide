using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class SkyBolt : MonoBehaviour
{
    Bolts bolts;
    ParticleSystem[] particles;
    AttackProfiles attackProfile;
    SpriteRenderer circle;
    PlayerAbilities playerAbilities;
    GameManager gm;
    [SerializeField] EventReference bigBoom;
    bool instantiatedCorrectly = false;

    public static SkyBolt Instantiate(GameObject prefab, Vector3 position, AttackProfiles attackProfile, PlayerAbilities playerAbilities)
    {
        SkyBolt skyBolt = Instantiate(prefab).GetComponent<SkyBolt>();
        skyBolt.attackProfile = attackProfile;
        float xOffset = Random.Range(-attackProfile.specialValue, attackProfile.specialValue);
        float zOffset = Random.Range(-attackProfile.specialValue, attackProfile.specialValue);
        skyBolt.transform.position = position + new Vector3(xOffset, 0, zOffset);
        skyBolt.FindTargetWithNavmesh();
        skyBolt.playerAbilities = playerAbilities;
        skyBolt.instantiatedCorrectly = true;
        return skyBolt;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!instantiatedCorrectly) Utils.IncorrectInitialization("SkyBolt");

        bolts = GetComponentInChildren<Bolts>();
        particles = GetComponentsInChildren<ParticleSystem>();
        circle = GetComponentInChildren<SpriteRenderer>();
        gm = GlobalEvents.instance.GetComponent<GameManager>();
        bolts.BoltsAway();
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        float delayTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(delayTime);
        bolts.BoltsFlash(transform.position, transform.position + Vector3.up * 10);
        yield return new WaitForSeconds(0.05f);
        Hit();
        circle.enabled = false;
        foreach (ParticleSystem particle in particles)
        {
            particle.Play();
        }
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void Hit()
    {
        FmodUtils.PlayOneShot(attackProfile.noHitSoundEvent, attackProfile.soundNoHitVolume, transform.position);
        FmodUtils.PlayOneShot(bigBoom, 0.5f, transform.position);
        if (attackProfile.screenShakeNoHit != Vector2.zero)
        {
            GlobalEvents.instance.ScreenShake(attackProfile.screenShakeNoHit.x, attackProfile.screenShakeNoHit.y);
        }

        //Utils.DrawDebugCircle(12, attackProfile.attackRange, transform.position);
        int damage = playerAbilities.DetermineAttackDamage(attackProfile);
        Utils.CircleHitbox(attackProfile, damage, transform.position, gm, playerAbilities);
    }

    void FindTargetWithNavmesh()
    {
        NavMeshHit outHit;
        if (NavMesh.SamplePosition(transform.position, out outHit, 0.1f, NavMesh.AllAreas))
        {
            transform.position = outHit.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
