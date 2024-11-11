using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] EnemyEvents enemyEvents;
    [SerializeField] RectMask2D mask;
    [SerializeField] float maxSize;
    [SerializeField] bool isBoss = false;

    private void Start()
    {
        if(enemyEvents == null)
        {
            enemyEvents = GetComponentInParent<EnemyEvents>();
        }
    }

    private void OnUpdateHealth(EnemyEvents sender, float healthRatio)
    {
        if(healthRatio < 0) healthRatio = 0;
        mask.padding = new Vector4(0, 0, Mathf.Lerp(maxSize, 0, healthRatio), 0);
    }

    private void OnBossKilled(object sender, System.EventArgs e)
    {
        if (isBoss)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        enemyEvents.OnUpdateHealth += OnUpdateHealth;
        GlobalEvents.instance.onBossKilled += OnBossKilled;
    }

    private void OnDisable()
    {
        enemyEvents.OnUpdateHealth -= OnUpdateHealth;
        GlobalEvents.instance.onBossKilled -= OnBossKilled;
    }
}
