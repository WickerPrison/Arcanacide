using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //This script is responsible for things that happen automatically without player input
    //such as health and stamina

    public int health;
    public int attackPower = 20;
    public float stamina;
    [SerializeField] GameObject healbarFill;
    [SerializeField] GameObject staminabarFill;

    float maxStamina = 100;
    float maxStaminaDelay = 1f;
    float staminaDelay;
    float staminaRate = 40;
    float healthbarScale = 1.555f;
    int maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        stamina = maxStamina;
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
        UpdateHealthbar();
    }

    public void LoseStamina(float amount)
    {
        stamina -= amount;
        if(stamina < 0)
        {
            stamina = 0;
        }
        staminaDelay = maxStaminaDelay;
    }

    void UpdateHealthbar()
    {
        float healthRatio = (float)health / (float)maxHealth;
        healbarFill.transform.localScale = new Vector3(healthRatio * healthbarScale, healbarFill.transform.localScale.y, healbarFill.transform.localScale.z);
    }

    void UpdateStaminaBar()
    {
        float staminaRatio = stamina / maxStamina;
        staminabarFill.transform.localScale = new Vector3(staminaRatio * healthbarScale, staminabarFill.transform.localScale.y, staminabarFill.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        //If the player reaches 0 health, restart the current scene
        if(health <= 0 || transform.position.y < -20)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(staminaDelay > 0)
        {
            staminaDelay -= Time.deltaTime;
        }
        else if(stamina < maxStamina)
        {
            stamina += Time.deltaTime * staminaRate;
        }

        UpdateStaminaBar();
    }
}
