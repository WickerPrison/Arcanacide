using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAlly : MonoBehaviour
{
    float poiseDamage = 20;
    [System.NonSerialized] public bool isShielded = false;
    SpriteRenderer shield;
    PlayerScript playerScript;
    EnemyScript enemyScript;

    private void Awake()
    {
        enemyScript = GetComponentInParent<EnemyScript>();
    }

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        shield = GetComponent<SpriteRenderer>();
    }

    public void ShieldOnOff(bool onOrOff)
    {
        shield.enabled = onOrOff;
        isShielded = onOrOff;
    }

    private void OnLosePoise(object sender, System.EventArgs e)
    {
        if (!isShielded) return;

        playerScript.LosePoise(poiseDamage);
        playerScript.StartStagger(1);
    }

    private void OnEnable()
    {
        enemyScript.OnLosePoise += OnLosePoise;
    }


    private void OnDisable()
    {
        enemyScript.OnLosePoise -= OnLosePoise;
    }

}
