using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingIndicator : MonoBehaviour
{
    [SerializeField] Transform attackPoint;

    private void Update()
    {
        transform.LookAt(attackPoint);
    }
}
