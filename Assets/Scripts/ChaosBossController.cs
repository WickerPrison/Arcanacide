using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[System.Serializable]
public class ChaosBossController : EnemyController
{
    [System.NonSerialized] public FacePlayer facePlayer;
    float fleeRadiusMin = 0;
    float fleeRadiusMax = 12;
    Vector3 fleePoint;
    [System.NonSerialized] public int phase = 1;
    float phaseTriggerPercent = 1.1f;
    float phaseTrigger;

    public override void Start()
    {
        base.Start();
        ChooseRandomPoint();
        facePlayer = GetComponent<FacePlayer>();
        phaseTrigger = enemyScript.maxHealth * phaseTriggerPercent;
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (phase == 1 && enemyScript.health < phaseTrigger)
        {
            phase = 2;
        }

        if (state == EnemyState.SPECIAL)
        {
            facePlayer.SetDestination(fleePoint);
            float distance = Vector3.Distance(transform.position, fleePoint);
            if (distance <= navAgent.stoppingDistance)
            {
                state = EnemyState.IDLE;
            }
        }

        if (navAgent.enabled)
        {
            navAgent.SetDestination(fleePoint);
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    void RunAway()
    {
        state = EnemyState.SPECIAL;
        ChooseRandomPoint();

        if (navAgent.enabled)
        {
            navAgent.SetDestination(fleePoint);
        }
    }

    void ChooseRandomPoint()
    {
        int xDir = Random.Range(1, 3);
        int yDir = Random.Range(1, 3);
        float xPos = Random.Range(fleeRadiusMin, fleeRadiusMax);
        float zPos = Random.Range(fleeRadiusMin, fleeRadiusMax);
        Vector3 startPos = new Vector3(xPos * Mathf.Pow(-1, xDir), 0, zPos * Mathf.Pow(-1, yDir));
        NavMeshHit hit;
        NavMesh.SamplePosition(startPos, out hit, 10, NavMesh.AllAreas);
        fleePoint = hit.position;
        if(Vector3.Distance(fleePoint, transform.position) < 5)
        {
            ChooseRandomPoint();
        }
    }

    public override void OnTakeDamage(object sender, System.EventArgs e)
    {
        base.OnTakeDamage(sender, e);
    }

    public override void EndStagger()
    {
        base.EndStagger();
        RunAway();
    }
}
