using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundFire : MonoBehaviour
{
    [SerializeField] GameObject fireTrailPrefab;
    public Transform target;
    NavMeshAgent navAgent;
    float fireTrailMaxTime = 0.2f;
    float fireTrailTime;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (fireTrailTime < 0)
        {
            FireTrail();
            fireTrailTime = fireTrailMaxTime;
        }
        else
        {
            fireTrailTime -= Time.deltaTime;
        }
        navAgent.SetDestination(target.position);
    }

    void FireTrail()
    {
        GameObject fireTrail;
        fireTrail = Instantiate(fireTrailPrefab);
        fireTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        fireTrail.transform.localScale = Vector3.Scale(fireTrail.transform.localScale, new Vector3(2f, 1f, 2f));
        FireTrail fireTrailScript = fireTrail.GetComponent<FireTrail>();
        fireTrailScript.duration = 2;
        fireTrailScript.damagePerSecond = 10;
    }
}
