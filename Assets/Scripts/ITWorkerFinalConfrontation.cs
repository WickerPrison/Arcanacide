using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITWorkerFinalConfrontation : MonoBehaviour
{
    [SerializeField] MapData mapData;
    EnemyEvents enemyEvents;

    private void OnEnable()
    {
        enemyEvents = GetComponent<EnemyEvents>();
        enemyEvents.OnDeath += EnemyEvents_OnDeath;
    }

    private void OnDisable()
    {
        enemyEvents.OnDeath -= EnemyEvents_OnDeath;
    }

    private void EnemyEvents_OnDeath(object sender, System.EventArgs e)
    {
        mapData.itWorkerQuestComplete = true;
    }
}
