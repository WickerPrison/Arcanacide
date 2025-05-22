using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStalagmite : MonoBehaviour
{
    [SerializeField] GameObject icicle;
    [SerializeField] GameObject puddle;
    [SerializeField] Collider collide;
    [SerializeField] MapData mapData;
    [SerializeField] bool showPuddle = true;

    private void Start()
    {
        Setup(mapData.ACOn);
    }

    void Setup(bool acOn)
    {
        icicle.SetActive(acOn);
        if (showPuddle)
        {
            puddle.SetActive(!acOn);
        }
        else
        {
            puddle.SetActive(false);
        }
        collide.enabled = acOn;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onSwitchAC += Global_onSwitchAC;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onSwitchAC -= Global_onSwitchAC;

    }

    private void Global_onSwitchAC(object sender, bool acOn)
    {
        Setup(acOn);
    }
}
