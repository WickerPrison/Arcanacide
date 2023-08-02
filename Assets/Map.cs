using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] GameObject mapPrefab;
    GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        map = Instantiate(mapPrefab);
        map.transform.SetParent(transform, false);
    }
}
