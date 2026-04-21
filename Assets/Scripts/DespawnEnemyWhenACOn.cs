using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DespawnEnemyWhenACOn : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] GameObject[] hideObjects;
    Vector3 away = Vector3.one * 100f;
    Vector3 spawnPos;
    EnemyController enemyController;
    NavMeshAgent navAgent;
    GameManager gm;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        navAgent = GetComponent<NavMeshAgent>();
        gm = GlobalEvents.instance.gameObject.GetComponent<GameManager>();
        spawnPos = transform.position;
        ShowEnemy(false);
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return null;
        ShowEnemy(true);
        if (mapData.ACOn)
        {
            DisableEnemy();
        }
    }

    void DisableEnemy()
    {
        transform.position = away;
        if(enemyController.state != EnemyState.UNAWARE)
        {
            gm.awareEnemies -= 1;
        }
        enemyController.state = EnemyState.DISABLED;
        navAgent.enabled = false;
        GlobalEvents.instance.DespawnEnemyAtLoad();
    }

    void EnableEnemy()
    {
        transform.position = spawnPos;
        enemyController.state = EnemyState.UNAWARE;
        navAgent.enabled = true;
    }

    void ShowEnemy(bool show)
    {
        if (hideObjects == null) return;
        foreach (GameObject hideObject in hideObjects)
        {
            hideObject.SetActive(show);
        }
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onSwitchAC += Global_onSwitchAC;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onSwitchAC -= Global_onSwitchAC;
    }

    private void Global_onSwitchAC(object sender, bool acOn)
    {
        if (acOn)
        {
            DisableEnemy();
        }
        else
        {
            EnableEnemy();
        }
    }
}
