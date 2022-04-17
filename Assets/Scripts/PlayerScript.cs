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
    InputManager im;
    PlayerController playerController;
    PlayerAnimation playerAnimation;
    PlayerSound sfx;
    float maxStaminaDelay = 1f;
    float staminaDelay;
    float staminaRate = 40;
    float healthbarScale = 1.555f;
    float maxPoise = 100;
    float poiseRate = 5;
    public float duckHealTimer = 0;
    float duckHealDuration = 2;
    float duckHealCounter = 0;
    float duckHealSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        altarDirectory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AltarDirectory>();
        gm = altarDirectory.gameObject.GetComponent<GameManager>();
        im = altarDirectory.gameObject.GetComponent<InputManager>();
        stamina = playerData.MaxStamina();
        poise = maxPoise;
        playerController = GetComponent<PlayerController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        sfx = GetComponentInChildren<PlayerSound>();
        UpdateHealthbar();
    }

    public void LoseHealth(int damage)
    {
        if (!playerController.shield)
        {
            playerData.health -= damage;
            if(playerData.health <= 0)
            {
                im.DisableAll();
                playerController.preventInput = true;
                YouDied youDied = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<YouDied>();
                youDied.playerScript = this;
                youDied.ShowMessage();
            }
            else
            {
                UpdateHealthbar();
            }
        }
    }

    public void MaxHeal()
    {
        playerData.health = playerData.MaxHealth();
        UpdateHealthbar();
    }

    public void PartialHeal(int healAmount)
    {
        playerData.health += healAmount;
        if(playerData.health > playerData.MaxHealth())
        {
            playerData.health = playerData.MaxHealth();
        }
        UpdateHealthbar();
    }

    public void DuckHeal()
    {
        duckHealTimer = duckHealDuration;
        duckHealSpeed = playerData.MaxHealth() / duckHealDuration;
        sfx.Heal();
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
        float healthRatio = (float)playerData.health / (float)playerData.MaxHealth();
        healbarFill.transform.localScale = new Vector3(healthRatio * healthbarScale, healbarFill.transform.localScale.y, healbarFill.transform.localScale.z);
    }

    void UpdateStaminaBar()
    {
        float staminaRatio = stamina / playerData.MaxStamina();
        staminabarFill.transform.localScale = new Vector3(staminaRatio * healthbarScale, staminabarFill.transform.localScale.y, staminabarFill.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(staminaDelay > 0)
        {
            staminaDelay -= Time.deltaTime;
        }
        else if(stamina < playerData.MaxStamina())
        {
            stamina += Time.deltaTime * staminaRate;
            if(playerData.path == "Dying" && playerController.pathActive)
            {
                stamina += Time.deltaTime * staminaRate;
            }
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

    public void Death()
    {
        MaxHeal();
        playerData.healCharges = playerData.maxHealCharges;
        playerData.hasSpawned = false;
        playerData.duckCD = 0;
        playerData.lostMoney = playerData.money;
        playerData.money = 0;
        mapData.doorNumber = 0;
        mapData.deadEnemies.Clear();
        mapData.usedChargingStations.Clear();
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
        playerData.healCharges = playerData.maxHealCharges;
        playerData.hasSpawned = false;
        gm.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
