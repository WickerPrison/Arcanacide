using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoint : MonoBehaviour
{
    public FollowPoint next;
    FollowPath FollowPath;
    FollowPath followPath
    {
        get
        {
            if (FollowPath == null) FollowPath = GetComponentInParent<FollowPath>();
            return FollowPath;
        }
    }

    private void OnEnable()
    {
        if (!followPath.followPoints.Contains(this))
        {
            followPath.followPoints.Add(this);
        }
    }

    private void OnDisable()
    {
        if (followPath.followPoints.Contains(this))
        {
            followPath.followPoints.Remove(this);
        }
    }
}
