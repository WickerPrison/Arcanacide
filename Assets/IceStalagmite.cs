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
        icicle.SetActive(mapData.ACOn);
        if (showPuddle)
        {
            puddle.SetActive(!mapData.ACOn);
        }
        else
        {
            puddle.SetActive(false);
        }
        collide.enabled = mapData.ACOn;
    }
}
