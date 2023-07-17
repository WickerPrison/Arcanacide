using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] Vector3[] positions;
    [SerializeField] List<Transform> listItems = new List<Transform>();
    [SerializeField] Transform description;
    [SerializeField] Transform childDescription;
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI currentMoney;
    [SerializeField] TextMeshProUGUI requiredMoneyText;
    [SerializeField] TextMeshProUGUI strengthAmount;
    [SerializeField] TextMeshProUGUI dexterityAmount;
    [SerializeField] TextMeshProUGUI vitalityAmount;
    [SerializeField] TextMeshProUGUI dedicationAmount;
    [SerializeField] LevelUpButton strengthButton;
    [SerializeField] LevelUpButton dexterityButton;
    [SerializeField] LevelUpButton vitalityButton;
    [SerializeField] LevelUpButton dedicationButton;
    public RestMenuButtons restMenuScript;
    int requiredMoney;
    public int altarNumber;
    public Transform spawnPoint;
    PlayerControls controls;
    SoundManager sm;

    float transitionTime = 0.1f;
    float descriptionTransitionTime = 0.1f;
    Vector3 descriptScaleMax;
    Vector3 descriptScaleMin;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.performed += ctx => OpenRestMenu();
    }

    private void Start()
    {
        descriptScaleMax = childDescription.localScale;
        descriptScaleMin = new Vector3(descriptScaleMax.x, 0, descriptScaleMax.z);
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(strengthButton.gameObject);
        UpdateText();   
    }

    public void SelectItem(Transform item)
    {
        listItems.Remove(description);
        listItems.Insert( listItems.IndexOf(item) + 1, description);
        StopAllCoroutines();
        StartCoroutine(MoveItems());
    }

    IEnumerator MoveItems()
    {
        childDescription.localScale = descriptScaleMin;
        float timer = transitionTime;
        List<Vector3> initialPositions = new List<Vector3>();
        foreach(Transform item in listItems)
        {
            initialPositions.Add(item.localPosition);
        }

        while(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;
            for (int i = 0; i < listItems.Count; i++)
            {
                listItems[i].transform.localPosition = Vector3.Lerp(positions[i], initialPositions[i], ratio);
            }
            yield return new WaitForEndOfFrame();
        }

        timer = descriptionTransitionTime;
        while(timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;
            childDescription.localScale = Vector3.Lerp(descriptScaleMax, descriptScaleMin, ratio);
            yield return new WaitForEndOfFrame();
        }
    }

    public void HideDescription()
    {
        listItems.Remove(description);
        listItems.Add(description);

        StartCoroutine(ShrinkDescription());
    }

    IEnumerator ShrinkDescription()
    {
        float timer = transitionTime;
        List<Vector3> initialPositions = new List<Vector3>();
        Vector3 initialScale = childDescription.localScale;
        foreach (Transform item in listItems)
        {
            initialPositions.Add(item.localPosition);
        }

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            float ratio = timer / transitionTime;
            for (int i = 0; i < listItems.Count; i++)
            {
                listItems[i].transform.localPosition = Vector3.Lerp(positions[i], initialPositions[i], ratio);
            }

            childDescription.localScale = Vector3.Lerp(descriptScaleMin, initialScale, ratio);

            yield return new WaitForEndOfFrame();
        }
    }

    public void IncreaseStrength()
    {
        sm.ButtonSound();
        RequiredMoney();
        if(playerData.money >= requiredMoney)
        {
            strengthButton.LevelUp();
            playerData.money -= requiredMoney;
            playerData.strength += 1;
            UpdateText();
        }
        else strengthButton.FailToLevelUp();
    }

    public void IncreaseDexterity()
    {
        sm.ButtonSound();
        RequiredMoney();
        if (playerData.money >= requiredMoney)
        {
            dexterityButton.LevelUp();
            playerData.money -= requiredMoney;
            playerData.dexterity += 1;
            UpdateText();
        }
        else dexterityButton.FailToLevelUp();
    }

    public void IncreaseVitality()
    {
        sm.ButtonSound();
        RequiredMoney();
        if (playerData.money >= requiredMoney)
        {
            vitalityButton.LevelUp();
            playerData.money -= requiredMoney;
            playerData.vitality += 1;
            UpdateText();
        }
        else vitalityButton.FailToLevelUp();
    }

    public void IncreaseDedication()
    {
        sm.ButtonSound();
        RequiredMoney();
        if (playerData.money >= requiredMoney)
        {
            dedicationButton.LevelUp();
            playerData.money -= requiredMoney;
            playerData.dedication += 1;
            UpdateText();
        }
        else dedicationButton.FailToLevelUp();
    }

    public void OpenRestMenu()
    {
        /*
        restMenu = Instantiate(restMenuPrefab);
        RestMenuButtons restMenuScript = restMenu.GetComponent<RestMenuButtons>();
        restMenuScript.altarNumber = altarNumber;
        restMenuScript.spawnPoint = spawnPoint;
        */
        sm.ButtonSound();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(restMenuScript.firstButton);
        restMenuScript.controls.Enable();
        Destroy(gameObject);
    }

    void UpdateText()
    {
        currentMoney.text = "Current Money: " + playerData.money.ToString();
        RequiredMoney();
        requiredMoneyText.text = "Level Up Cost: " + requiredMoney.ToString();
        strengthAmount.text = playerData.strength.ToString();
        dexterityAmount.text = playerData.dexterity.ToString();
        vitalityAmount.text = playerData.vitality.ToString();
        dedicationAmount.text = playerData.dedication.ToString();
    }

    void RequiredMoney()
    {
        requiredMoney = Mathf.RoundToInt(4 + Mathf.Pow(playerData.GetLevel(), 2));
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
