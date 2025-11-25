using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    public EnemyEvents enemyEvents;
    [SerializeField] RectMask2D mask;
    [SerializeField] RectMask2D delayMask;
    [SerializeField] float maxSize;
    [SerializeField] bool isBoss = false;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    float delay;
    [SerializeField] float speed;
    bool buffer = false;

    private void Awake()
    {
        if (enemyEvents == null)
        {
            enemyEvents = GetComponentInParent<EnemyEvents>();
        }
    }

    private void OnUpdateHealth(EnemyEvents sender, float healthRatio)
    {
        if(healthRatio < 0) healthRatio = 0;
        if (!buffer)
        {
            StopAllCoroutines();
            StartCoroutine(HealthbarDelay());
        }
        else delay = 0.5f;
        mask.padding = new Vector4(0, 0, Mathf.Lerp(maxSize, 0, healthRatio), 0);
    }

    IEnumerator HealthbarDelay()
    {
        delayMask.padding = mask.padding;
        buffer = true;
        delay = 0.5f;
        while(delay > 0)
        {
            delay -= Time.deltaTime;
            yield return endOfFrame;
        }

        while(delayMask.padding.z < mask.padding.z)
        {
            delayMask.padding += new Vector4(0, 0, Time.deltaTime * speed, 0);
            yield return endOfFrame;
        }
        buffer = false;
    }

    private void OnBossKilled(object sender, System.EventArgs e)
    {
        DisableBossHealthbar();
    }

    private void OnMinibossKilled(object sender, System.EventArgs e)
    {
        DisableBossHealthbar();
    }


    void DisableBossHealthbar()
    {
        if (isBoss)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        enemyEvents.OnUpdateHealth += OnUpdateHealth;
        enemyEvents.OnHideBossHealthbar += OnBossKilled;
        GlobalEvents.instance.onBossKilled += OnBossKilled;
        GlobalEvents.instance.onMinibossKilled += OnMinibossKilled;
    }

    private void OnDisable()
    {
        enemyEvents.OnUpdateHealth -= OnUpdateHealth;
        enemyEvents.OnHideBossHealthbar -= OnBossKilled;
        GlobalEvents.instance.onBossKilled -= OnBossKilled;
        GlobalEvents.instance.onMinibossKilled -= OnMinibossKilled;
    }
}
