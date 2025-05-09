using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinibossAnimationEvents : MonoBehaviour
{
    MinibossAbilities abilities;
    EnemyScript enemyScript;
    EnemyController enemyController;
    MinibossEvents minibossEvents;


    private void Awake()
    {
        minibossEvents = GetComponentInParent<MinibossEvents>(); 
    }

    private void Start()
    {
        abilities = GetComponentInParent<MinibossAbilities>();
        enemyScript = abilities.GetComponent<EnemyScript>();
        enemyController = enemyScript.GetComponent<EnemyController>();
    }

    public void FireMissile()
    {
        abilities.FireMissiles(abilities.missilePattern);
    }

    public void StartAttackDash(string endAnimation)
    {
        abilities.AttackDash(endAnimation);
    }

    public void FirePlasmaShot()
    {
        abilities.FirePlasmaShot();
    }

    public void StartChestLaser()
    {
        abilities.StartLaser();
    }

    public void DefeatDialogue()
    {
        Dialogue dialogue = GetComponent<Dialogue>();
        dialogue.StartConversation();
    }

    public void ThrustersOn()
    {
        minibossEvents.ThrustersOn();
    }

    public void ThrustersOff()
    {
        minibossEvents.ThrustersOff();
    }

    public void FlyUp()
    {
        abilities.FlyUp();
    }

    public void FlyAway()
    {
        StartCoroutine(Flying());
    }

    IEnumerator Flying()
    {
        while(abilities.transform.position.y < 20)
        {
            abilities.transform.position += new Vector3(0, 25 * Time.deltaTime, 0);
            yield return null;
        }

        enemyScript.Death();
        enemyController.Death();
    }

    private void OnEnable()
    {
        minibossEvents.OnStagger += EnemyEvents_OnStagger;
    }

    private void OnDisable()
    {
        minibossEvents.OnStagger -= EnemyEvents_OnStagger;
    }

    private void EnemyEvents_OnStagger(object sender, EventArgs e)
    {
        minibossEvents.ThrustersOff();
    }
}
