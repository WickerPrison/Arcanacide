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
    public string emblemName;
    public int cost = 100;
    SoundManager sm;

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
            emblemNameText.color = mapData.floorColor;
            descriptionText.text = emblemLibrary.GetDescription(emblemName);
            if (playerData.emblems.Contains(emblemName))
            {
                costText.text = "Sold Out";
            }
            else
            {
                costText.text = "$" + cost.ToString();
            }
        }
        else
        {
            emblemNameText.color = Color.white;
        }
    }
}
