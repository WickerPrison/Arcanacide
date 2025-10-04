using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPoints : MonoBehaviour
{
    [SerializeField] Transform[] spinPoints;

    public Vector3 GetPosition(int index)
    {
        return spinPoints[index].position;
    }
}
