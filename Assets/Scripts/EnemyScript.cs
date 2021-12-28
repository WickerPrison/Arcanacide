using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //this script controls the automatic workings of the enemy like health 

    public int health;
    [SerializeField] GameObject healthbar;
    [SerializeField] float healthbarScale;
    [SerializeField] int enemyID;
    [SerializeField] MapData mapData;
    EnemyController enemyController;
    GameManager gm;
    [SerializeField] int maxHealth = 100;

    private void Awake()
    {
        if (mapData.deadEnemies.Contains(enemyID))
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        enemyController = GetComponent<EnemyController>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.numberOfEnemies += 1;
    }

    public void LoseHealth(int damage)
    {
        if (!enemyController.SwordClash())
        {
            health -= damage;
            UpdateHealthbar();
            enemyController.OnHit();
        }
    }

    void UpdateHealthbar()
    {
        float healthbarRatio = (float)health / (float)maxHealth;
        if(health > 0)
        {
            healthbar.transform.localScale = new Vector3(healthbarRatio * healthbarScale, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
        }
        else
        {
            healthbar.transform.localScale = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        mapData.deadEnemies.Add(enemyID);
        gm.numberOfEnemies -= 1;
        if (enemyController.detectionTrigger)
        {
            gm.awareEnemies -= 1;
        }
        Destroy(gameObject);
    }
}
