using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossAnimationEvents : MonoBehaviour
{
    MinibossAbilities abilities;
    EnemyScript enemyScript;
    EnemyController enemyController;

    private void Start()
    {
        abilities = GetComponentInParent<MinibossAbilities>();
        enemyScript = abilities.GetComponent<EnemyScript>();
        enemyController = enemyScript.GetComponent<EnemyController>();
    }

    public void FireMissile()
    {
        abilities.FireMissiles();
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
}
