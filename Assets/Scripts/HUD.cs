using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject healbarFill;
    [SerializeField] GameObject staminabarFill;
    [SerializeField] GameObject manaBarFill;
    [SerializeField] GameObject manaBarCrack;
    [SerializeField] TextMeshProUGUI healCounter;
    PlayerScript playerScript;
    float healthbarScale = 1.555f;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if (playerData.healCharges < 0)
        {
            healCounter.gameObject.SetActive(false);
            manaBarCrack.SetActive(true);
        }
        else
        {
            healCounter.gameObject.SetActive(true);
            manaBarCrack.SetActive(false);
        }
        UpdateHealthbar();
        UpdateStaminaBar();
        UpdateManaBar();
        healCounter.text = playerData.healCharges.ToString();
    }

    void UpdateHealthbar()
    {
        float healthRatio = (float)playerData.health / (float)playerData.MaxHealth();
        if(playerData.health < 0)
        {
            healthRatio = 0;
        }
        healbarFill.transform.localScale = new Vector3(healthRatio * healthbarScale, healbarFill.transform.localScale.y, healbarFill.transform.localScale.z);
    }

    void UpdateStaminaBar()
    {
        float staminaRatio = playerScript.stamina / playerData.MaxStamina();
        staminabarFill.transform.localScale = new Vector3(staminaRatio * healthbarScale, staminabarFill.transform.localScale.y, staminabarFill.transform.localScale.z);
    }

    void UpdateManaBar()
    {
        float manaRatio = (float)playerData.mana / (float)playerData.maxMana;
        if (manaRatio >= 0)
        {
            manaBarFill.transform.localScale = new Vector3(manaBarFill.transform.localScale.x, manaRatio, manaBarFill.transform.localScale.z);
        }
    }
}
