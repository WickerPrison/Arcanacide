using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOffhandWeapon : MonoBehaviour
{
    [SerializeField] GameObject offhandWeapon;

    private void OnEnable()
    {
        offhandWeapon.SetActive(true);
    }

    private void OnDisable()
    {
        offhandWeapon.SetActive(false);
    }
}
