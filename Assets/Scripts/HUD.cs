using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject healbarFill;
    //[SerializeField] GameObject staminabarFill;
    [SerializeField] RectTransform staminaBarCover;
    [SerializeField] GameObject manaGemIcon;
    [SerializeField] GameObject manaBarFill;
    [SerializeField] GameObject manaBarCrack;
    [SerializeField] TextMeshProUGUI healCounter;
    public List<Sprite> gemSprites = new List<Sprite>();
    [SerializeField] Sprite unbrokenGem;
    public Image gemImage;
    [SerializeField] Image gemProtection;
    PlayerScript playerScript;
    float healthbarScale = 1;
    Canvas canvas;
    Camera mainCamera;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;

        if (!playerData.hasHealthGem)
        {
            manaGemIcon.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateHealthbar();
        UpdateStaminaBar();
        UpdateManaBar();
        UpdateGemCracks();
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
        //staminabarFill.transform.localScale = new Vector3(staminaRatio * healthbarScale, staminabarFill.transform.localScale.y, staminabarFill.transform.localScale.z);
        staminaBarCover.localScale = new Vector3(1 - staminaRatio, staminaBarCover.localScale.y, staminaBarCover.localScale.z);
    }

    void UpdateManaBar()
    {   
        float manaRatio = (float)playerData.mana / (float)playerData.maxMana;
        if (manaRatio >= 0)
        {
            manaBarFill.transform.localScale = new Vector3(manaBarFill.transform.localScale.x, manaRatio, manaBarFill.transform.localScale.z);
        }
    }

    void UpdateGemCracks()
    {
        if(playerData.healCharges >= playerData.maxHealCharges)
        {
            gemImage.sprite = unbrokenGem;
            gemProtection.enabled = playerData.healCharges > playerData.maxHealCharges;
        }
        else
        {
            gemImage.sprite = gemSprites[playerData.healCharges + 1];
            gemProtection.enabled = false;
        }
    }
}
