using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
        transform.parent = null;
    }

    private void FixedUpdate()
    {
        if (parentTransform != null)
        {
            transform.position = parentTransform.position;
        }
    }
}
