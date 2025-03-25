using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockLocalPosition : MonoBehaviour
{
    enum LockAxis
    {
        X, Y, Z
    }
    [SerializeField] LockAxis lockAxis;

    private void OnValidate()
    {
        Debug.Log(lockAxis);
        switch (lockAxis)
        {
            case LockAxis.X:
                transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
                break;
            case LockAxis.Y:
                transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
                break;
            case LockAxis.Z:
                transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
                break;
        }
    }
}
