using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StalagmiteHolder : MonoBehaviour
{
    StalagmiteAttack[] stalagmites;
    SortedDictionary<float, StalagmiteAttack> stalagmiteDict = new SortedDictionary<float, StalagmiteAttack>();
    [SerializeField] float waveSpeed;
    EnemyEvents enemyEvents;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EnemyScript enemyOfOrigin = GetComponentInParent<EnemyScript>();
        stalagmites = GetComponentsInChildren<StalagmiteAttack>();
        foreach(StalagmiteAttack stalagmite in stalagmites)
        {
            stalagmiteDict.Add(Vector3.Distance(transform.position, stalagmite.transform.position), stalagmite);
            stalagmite.enemyOfOrigin = enemyOfOrigin;
        }
    }

    public void TriggerAll()
    {
        foreach(StalagmiteAttack stalagmite in stalagmites)
        {
            stalagmite.Trigger();
        }
    }

    public void TriggerWave()
    {
        StartCoroutine(Wave());
    }

    IEnumerator Wave()
    {
        float currentVal = 0;
        List<float> remainingStalagmites = stalagmiteDict.Keys.ToList();
        while(remainingStalagmites.Count > 0)
        {
            currentVal += Time.deltaTime * waveSpeed;
            while(remainingStalagmites.Count > 0 && currentVal > remainingStalagmites[0])
            {
                stalagmiteDict[remainingStalagmites[0]].Trigger();
                remainingStalagmites.RemoveAt(0);
            }
            yield return null;
        }
    }

    void CancelWave()
    {
        StopAllCoroutines();
        foreach(StalagmiteAttack stalagmite in stalagmites)
        {
            stalagmite.CancelStalagmite();
        }
    }

    private void OnEnable()
    {
        enemyEvents.OnStagger += EnemyEvents_OnStagger;
        enemyEvents.OnStartDying += EnemyEvents_OnStartDying;
    }

    private void OnDisable()
    {
        enemyEvents.OnStagger -= EnemyEvents_OnStagger;
        enemyEvents.OnStartDying -= EnemyEvents_OnStartDying;
    }

    private void EnemyEvents_OnStagger(object sender, System.EventArgs e)
    {
        CancelWave();
    }

    private void EnemyEvents_OnStartDying(object sender, System.EventArgs e)
    {
        CancelWave();
    }
}
