using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireWaveTrail : MonoBehaviour
{
    private bool initializedCorrectly = false;
    [System.NonSerialized] public PlayerTrailManager trailManager;
    PathTrail[] pathTrails;
    AttackProfiles attackProfile;

    public static PlayerFireWaveTrail Instantiate(GameObject prefab, Vector3 spawnPosition, Quaternion spawnRotation, PlayerTrailManager trailManager, AttackProfiles attackProfile)
    {
        PlayerFireWaveTrail trail = Instantiate(prefab).GetComponent<PlayerFireWaveTrail>();
        trail.transform.position = spawnPosition;
        trail.transform.rotation = spawnRotation;
        trail.trailManager = trailManager;
        trail.attackProfile = attackProfile;
        trail.initializedCorrectly = true;
        trail.pathTrails = trail.GetComponentsInChildren<PathTrail>();
        foreach (PathTrail pathTrail in trail.pathTrails)
        {
            pathTrail.trailManager = trailManager;
            pathTrail.attackProfile = attackProfile;
            pathTrail.initializedCorrectly = true;
        }
        return trail;
    }

    private void Start()
    {
        if (!initializedCorrectly)
        {
            Utils.IncorrectInitialization("PlayerFireWaveTrail");
        }

        StartCoroutine(DeathTimer((attackProfile.durationDOT + 2) * 1.5f));
    }

    IEnumerator DeathTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
