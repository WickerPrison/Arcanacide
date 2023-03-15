using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOtherClaw : MonoBehaviour
{
    [SerializeField] GameObject otherClaws;

    private void OnEnable()
    {
        otherClaws.SetActive(true);
    }

    private void OnDisable()
    {
        otherClaws.SetActive(false);
    }
}
