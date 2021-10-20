using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //this script controls the automatic workings of the enemy like health 

    public int health;
    [SerializeField] GameObject healthbar;
    [SerializeField] float healthbarScale;

    int maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
        UpdateHealthbar();
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
            Destroy(gameObject);
        }
    }
}
