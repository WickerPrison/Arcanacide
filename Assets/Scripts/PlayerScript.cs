using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    //This script is responsible for things that happen automatically without player input
    //such as health and stamina

    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    public float stamina;
    public float poise;
    [SerializeField] GameObject healbarFill;
    [SerializeField] GameObject staminabarFill;
    AltarDirectory altarDirectory;
    GameManager gm;
    PlayerController playerController;
    PlayerAnimation playerAnimation;
    float maxStaminaDelay = 1f;
    float staminaDelay;
    float staminaRate = 40;
    float healthbarScale = 1.555f;
    float maxPoise = 100;
    float poiseRate = 5;
    float duckHealTimer = 0;
    float duckHealDuration = 2;
    float duckHealCounter = 0;
    float duckHealSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        altarDirectory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AltarDirectory>();
        gm = altarDirectory.gameObject.GetComponent<GameManager>();
        stamina = playerData.maxStamina;
        poise = maxPoise;
        playerController = GetComponent<PlayerController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        UpdateHealthbar();
    }

    public void LoseHealth(int damage)
    {
        if (!playerController.shield)
        {
            playerData.health -= damage;
            UpdateHealthbar();
        }
    }

    public void MaxHeal()
    {
        playerData.health = playerData.maxHealth;
        UpdateHealthbar();
    }

    void PartialHeal(int healAmount)
    {
        playerData.health += healAmount;
        if(playerData.health > playerData.maxHealth)
        {
            playerData.health = playerData.maxHealth;
        }
        UpdateHealthbar();
    }

    public void DuckHeal()
    {
        duckHealTimer = duckHealDuration;
        duckHealSpeed = playerData.maxHealth / duckHealDuration;
        playerAnimation.StartBodyMagic();
    }

    public void LosePoise(float poiseDamage)
    {
        if (!playerController.shield)
        {
            if(playerController.stagger <= 0)
            {
                poise -= poiseDamage;
            }
            if(poise <= 0)
            {
                playerAnimation.PlayStagger();
                playerController.stagger = playerController.maxStaggered;
            }
        }
    }
    
    public void ResetPoise()
    {
        poise = maxPoise;
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
        float healthRatio = (float)playerData.health / (float)playerData.maxHealth;
        healbarFill.transform.localScale = new Vector3(healthRatio * healthbarScale, healbarFill.transform.localScale.y, healbarFill.transform.localScale.z);
    }

    void UpdateStaminaBar()
    {
        float staminaRatio = stamina / playerData.maxStamina;
        staminabarFill.transform.localScale = new Vector3(staminaRatio * healthbarScale, staminabarFill.transform.localScale.y, staminabarFill.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        //If the player reaches 0 health, restart the current scene
        if(playerData.health <= 0 || transform.position.y < -20)
        {
            Death();   
        }

        if(staminaDelay > 0)
        {
            staminaDelay -= Time.deltaTime;
        }
        else if(stamina < playerData.maxStamina)
        {
            stamina += Time.deltaTime * staminaRate;
        }

        UpdateStaminaBar();

        if(poise < maxPoise)
        {
            poise += poiseRate * Time.deltaTime;
        }

        if(duckHealTimer > 0)
        {
            duckHealTimer -= Time.deltaTime;
            if(duckHealTimer <= 0)
            {
                playerAnimation.EndBodyMagic();
            }
            duckHealCounter += duckHealSpeed * Time.deltaTime;
            if(duckHealCounter >= 1)
            {
                int amount = Mathf.FloorToInt(duckHealCounter);
                PartialHeal(amount);
                duckHealCounter -= amount;
                UpdateHealthbar();
            }
        }
    }

    void Death()
    {
        MaxHeal();
        playerData.hasHealed = false;
        playerData.hasSpawned = false;
        playerData.duckCD = 0;
        playerData.lostMoney = playerData.money;
        playerData.money = 0;
        mapData.doorNumber = 0;
        mapData.deadEnemies.Clear();
        mapData.deathPosition = transform.position;
        mapData.deathRoom = SceneManager.GetActiveScene().name;
        gm.SaveGame();
        string sceneName = altarDirectory.GetSceneName(playerData.lastAltar);
        SceneManager.LoadScene(sceneName);
    }

    public void Rest()
    {
        MaxHeal();
        playerData.duckCD = 0;
        playerData.hasHealed = false;
        playerData.hasSpawned = false;
        gm.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
