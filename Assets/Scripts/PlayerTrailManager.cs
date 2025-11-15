using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailManager : MonoBehaviour
{
    [System.NonSerialized] public List<PathTrail> pathTrails = new List<PathTrail>();

    public bool HasSpace(Vector3 position, float radius)
    {
        if (pathTrails.Count == 0) return true;
        foreach (PathTrail pathTrail in pathTrails)
        {
            float enoughSpace = (radius + pathTrail.radius) * 0.7f;
            Debug.Log(enoughSpace);
            if (Vector3.Distance(pathTrail.transform.position, position) < enoughSpace)
            {
                return false;
            }
        }
        return true;
    }
}
