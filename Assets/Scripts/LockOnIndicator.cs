using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnIndicator : MonoBehaviour
{
    Vector3 startScale;
    bool isTargeted = false;
    float transitionTime = 0.2f;
    EnemyScript enemyScript;

    private void Start()
    {
        enemyScript = GetComponentInParent<EnemyScript>();
        startScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    public void SetTarget(bool getTargeted)
    {
        if (isTargeted == getTargeted) return;

        if (getTargeted)
        {
            StartCoroutine(StartTargeting());
        }
        else
        {
            StartCoroutine(EndTargeting());
        }
    }

    IEnumerator StartTargeting()
    {
        isTargeted = true;

        float timer = transitionTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer / transitionTime);
            yield return null;
        }

        transform.localScale = startScale;
    }

    IEnumerator EndTargeting()
    {
        isTargeted = false;

        float timer = transitionTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, startScale, timer / transitionTime);
            yield return null;
        }

        transform.localScale = Vector3.zero;
    }

    private void Global_onLockOnTarget(object sender, EnemyScript target)
    {
        if (target != null && target == enemyScript)
        {
            SetTarget(true);
        }
        else
        {
            SetTarget(false);
        }
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onLockOnTarget += Global_onLockOnTarget;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onLockOnTarget -= Global_onLockOnTarget;
    }
}
