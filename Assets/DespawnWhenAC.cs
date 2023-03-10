using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnWhenAC : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] bool despawnState;

    private void Awake()
    {
        if (mapData.ACOn == despawnState)
        {
            Destroy(gameObject);
        }
    }
}
