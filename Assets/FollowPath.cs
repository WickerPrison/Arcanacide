using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [System.NonSerialized] public List<FollowPoint> followPoints = new List<FollowPoint>();

    public FollowPoint GetClosestPoint(Vector3 startPos)
    {
        float dist = float.MaxValue;
        FollowPoint closest = null;
        foreach(FollowPoint point in followPoints)
        {
            float currentDist = Vector3.Distance(startPos, point.transform.position);
            if ( currentDist < dist)
            {
                dist = currentDist;
                closest = point;
            }
        }
        return closest;
    }
}
