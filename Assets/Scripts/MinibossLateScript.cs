using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossLateScript : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] int minibossVersion;
    MusicManager musicManager;

    private void Start()
    {
        musicManager = GlobalEvents.instance.GetComponent<MusicManager>();
        bool minibossDead = minibossVersion switch
        {
            1 => mapData.miniboss1Killed,
            2 => mapData.miniboss2Killed,
            3 => mapData.miniboss3Killed,
            4 => mapData.miniboss4Killed,
            _ => false
        };

        if (minibossDead)
        {
            musicManager.ChangeMusicState(MusicState.MAINLOOP);
            Destroy(gameObject);
        }
    }
   
}
