using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI emblemNameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject SoldOut;
    [SerializeField] GameObject dollarSign;
    public string emblemName;
    public int cost = 100;
    SoundManager sm;
    [SerializeField] Color selectedColor;

    public void ShopButtonPressed()
    {
        sm.ButtonSound();
        if (!playerData.emblems.Contains(emblemName) && playerData.money >= cost)
        {
            playerData.emblems.Add(emblemName);
            playerData.money -= cost;
        }
    }

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        StartCoroutine(SetEmblemName());
    }

    IEnumerator SetEmblemName()
    {
        yield return new WaitForEndOfFrame();
        emblemNameText.text = emblemName;   
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == gameObject)
        {
            emblemNameText.color = selectedColor;
            descriptionText.text = emblemLibrary.GetDescription(emblemName);
            if (playerData.emblems.Contains(emblemName))
            {
                dollarSign.SetActive(false);
                costText.gameObject.SetActive(false);
                SoldOut.SetActive(true);
            }
            else
            {
                dollarSign.SetActive(true);
                costText.text = cost.ToString();
                costText.gameObject.SetActive(true);
                SoldOut.SetActive(false);
            }
        }
        else
        {
            emblemNameText.color = Color.white;
        }
    }
}
