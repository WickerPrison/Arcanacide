using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHeightSetup : MonoBehaviour
{
    [SerializeField] float height;
    public bool lockHeight = true;

    private void OnDrawGizmosSelected()
    {
        if (lockHeight)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, height, transform.localPosition.z);
        }
    }
}
