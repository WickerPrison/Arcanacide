using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnWhenAC : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] bool despawnWhenOn;

    private void Awake()
    {
        if (mapData.ACOn == despawnWhenOn)
        {
            Destroy(gameObject);
        }
    }
}
