using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfCarolDead : MonoBehaviour
{
    [SerializeField] MapData mapData;

    // Start is called before the first frame update
    void Start()
    {
        if (mapData.electricBossKilled)
        {
            Destroy(gameObject);
        }
    }
}
