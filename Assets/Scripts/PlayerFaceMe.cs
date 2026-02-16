using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceMe : MonoBehaviour
{
    [SerializeField] SettingsData settingsData;

    // Start is called before the first frame update
    void Awake()
    {
        //if (settingsData.autoLock)
        //{
        //    EnemyScript enemyScript = GetComponent<EnemyScript>();
        //    GameObject.FindGameObjectWithTag("Player").GetComponent<LockOn>().TargetEnemy(enemyScript);
        //}
        //else
        //{
        //    // face player to enemy
        //}
    }
}
