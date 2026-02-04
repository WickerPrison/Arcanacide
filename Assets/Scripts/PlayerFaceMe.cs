using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyScript enemyScript = GetComponent<EnemyScript>();
        GameObject.FindGameObjectWithTag("Player").GetComponent<LockOn>().TargetEnemy(enemyScript);
    }
}
