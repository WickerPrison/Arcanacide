using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHalo : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] LightningHalo[] halos;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < halos.Length; i++)
        {
            if(i < mapData.carolsDeadFriends.Count)
            {
                halos[i].ShowRings(false);
                halos[i].enabled = false;
            }
        }   
    }
}
