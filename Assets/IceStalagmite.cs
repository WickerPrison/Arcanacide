using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStalagmite : MonoBehaviour
{
    [SerializeField] GameObject icicle;
    [SerializeField] GameObject puddle;
    [SerializeField] Collider collide;
    [SerializeField] MapData mapData;

    private void Start()
    {
        icicle.SetActive(mapData.ACOn);
        puddle.SetActive(!mapData.ACOn);
        collide.enabled = mapData.ACOn;
    }
}
