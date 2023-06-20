using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySucks : MonoBehaviour
{
    [SerializeField] Transform parent;
    Vector3 positionOffset;
    Vector3 rotationOffset;

    private void Start()
    {
        positionOffset = transform.localPosition;
        rotationOffset = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.position + positionOffset;
        transform.rotation = Quaternion.Euler(parent.rotation.eulerAngles + rotationOffset);
    }
}
