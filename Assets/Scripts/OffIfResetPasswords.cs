using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffIfResetPasswords : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] bool onWhenFinished;
    GameManager _gm;
    GameManager gm
    {
        get
        {
            if(_gm == null)
            {
                _gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            }
            return _gm;
        }
    }


    void Start()
    {
        if (mapData.itWorkerQuestComplete)
        {
            gameObject.SetActive(false);
            RemoveFromEnemiesList();
            return;
        }

        bool isFinished = mapData.resetPasswords != null && mapData.resetPasswords.Count == 4;
        bool shouldBeOn = onWhenFinished == isFinished;
        gameObject.SetActive(shouldBeOn);
        if (!shouldBeOn)
        {
            RemoveFromEnemiesList();
        }
    }

    void RemoveFromEnemiesList()
    {
        EnemyScript enemyScript = GetComponent<EnemyScript>();
        if(enemyScript != null)
        {
            gm.enemies.Remove(enemyScript);
        }
    }
}
