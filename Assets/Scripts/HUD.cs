using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] Image gemBackground;
    [SerializeField] GameObject manaGemIcon;
    [SerializeField] GameObject manaBarFill;
    [SerializeField] RectMask2D mask;
    [SerializeField] Material youDiedTextMaterial;
    [SerializeField] GameObject map;
    [SerializeField] ScreenMessage maxManaMessage;
    public List<Sprite> gemSprites = new List<Sprite>();
    [SerializeField] Sprite unbrokenGem;
    public Image gemImage;
    [SerializeField] Image gemProtection;
    Canvas canvas;
    Camera mainCamera;
    InputManager im;
    bool mapOpen = false;

    private void Start()
    {
        im = GlobalEvents.instance.GetComponent<InputManager>();
        im.controls.Gameplay.Map.performed += ctx => Map();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = mainCamera;

        youDiedTextMaterial.SetColor("_OutlineColor", mapData.floorColor);

        gemImage.material = new Material(gemImage.material);
        UpdateGemCracks();

        if (!playerData.hasHealthGem)
        {
            manaGemIcon.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateManaBar();
    }

    void UpdateManaBar()
    {   
        float manaRatio = (float)playerData.mana / (float)playerData.maxMana;
        if (manaRatio >= 0)
        {
            manaBarFill.transform.localScale = new Vector3(manaBarFill.transform.localScale.x, manaRatio, manaBarFill.transform.localScale.z);
        }
    }

    private void onGemUsed(object sender, System.EventArgs e)
    {
        StartCoroutine(GemFlash());
    }

    IEnumerator GemFlash()
    {
        gemImage.material.SetFloat("_BlackToWhite", 1);
        gemBackground.color = Color.white;
        UpdateGemCracks();
        yield return new WaitForSeconds(0.2f);
        gemImage.material.SetFloat("_BlackToWhite", 0);
        gemBackground.color = Color.black;
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

    void Map()
    {
        if (mapOpen)
        {
            map.SetActive(false);
        }
        else
        {
            map.SetActive(true);
        }

        mapOpen = !mapOpen;
    }

    public void MaxManaIncreased()
    {
        maxManaMessage.ShowMessage();
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onGemUsed += onGemUsed;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onGemUsed -= onGemUsed;
    }
}
