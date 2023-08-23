using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] GameObject[] mapPrefabs;
    public Dictionary<int, GameObject> mapPrefabDict;
    GameObject map;

    private void Awake()
    {
        mapPrefabDict = new Dictionary<int, GameObject>()
        {
            {1, mapPrefabs[0] },
            {2, mapPrefabs[1] },
            {3, mapPrefabs[2]},
            {5, mapPrefabs[3]}
        };                
    }

    // Start is called before the first frame update
    void Start()
    {
        map = Instantiate(mapPrefabDict[mapData.floor]);
        map.transform.SetParent(transform, false);
    }
}
