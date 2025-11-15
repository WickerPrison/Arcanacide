using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireWaveTrail : MonoBehaviour
{
    private bool initializedCorrectly = false;
    [System.NonSerialized] public PlayerTrailManager trailManager;
    PathTrail[] pathTrails;

    public static PlayerFireWaveTrail Instantiate(GameObject prefab, Vector3 spawnPosition, Quaternion spawnRotation, PlayerTrailManager trailManager)
    {
        PlayerFireWaveTrail trail = Instantiate(prefab).GetComponent<PlayerFireWaveTrail>();
        trail.transform.position = spawnPosition;
        trail.transform.rotation = spawnRotation;
        trail.trailManager = trailManager;
        trail.initializedCorrectly = true;
        trail.pathTrails = trail.GetComponentsInChildren<PathTrail>();
        foreach (PathTrail pathTrail in trail.pathTrails)
        {
            pathTrail.trailManager = trailManager;
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
    }
}
