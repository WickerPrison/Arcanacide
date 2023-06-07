using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IceGolemAnimationEvents : MonoBehaviour
{
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] IceGolemController iceGolemController;
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    float defaultAcceleration;
    float defaultSpeed;
    float defaultStoppingDistance;

    float chargingTimer;
    float chargingTimerMax = 1.5f;

    private void Start()
    {
        defaultAcceleration = navAgent.acceleration;
        defaultSpeed = navAgent.speed;
        defaultStoppingDistance = navAgent.stoppingDistance;
    }

    private void Update()
    {
        if(chargingTimer > 0)
        {
            chargingTimer -= Time.deltaTime;
        }
        frontAnimator.SetFloat("ChargingTimer", chargingTimer);
        backAnimator.SetFloat("ChargingTimer", chargingTimer);
    }

    public void StartCharge()
    {
        navAgent.enabled = true;
        navAgent.acceleration = 15;
        navAgent.speed = 9;
        navAgent.stoppingDistance = 0;
        iceGolemController.state = EnemyState.SPECIAL;
        chargingTimer = chargingTimerMax;
    }

    public void EndCharge()
    {
        navAgent.enabled = false;
        navAgent.acceleration = defaultAcceleration;
        navAgent.speed = defaultSpeed;
        navAgent.stoppingDistance = defaultStoppingDistance;
        iceGolemController.state = EnemyState.IDLE;
        chargingTimer = 0;
    }
}
