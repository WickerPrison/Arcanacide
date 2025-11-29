using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireCircle : MonoBehaviour
{
    private bool initializedCorrectly = false;
    [System.NonSerialized] public PlayerTrailManager trailManager;
    PathTrail[] pathTrails;
    AttackProfiles attackProfile;

    public static PlayerFireCircle Instantiate(GameObject prefab, Vector3 spawnPosition, PlayerTrailManager trailManager, AttackProfiles attackProfile)
    {
        PlayerFireCircle trail = Instantiate(prefab).GetComponent<PlayerFireCircle>();
        trail.transform.position = spawnPosition;
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
            Utils.IncorrectInitialization("PlayerFireCircle");
        }

        StartCoroutine(DeathTimer((attackProfile.durationDOT + 2) * 1.5f));
    }

    IEnumerator DeathTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
