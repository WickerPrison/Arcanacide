using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu2Button : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Image[] backgrounds;
    [SerializeField] TextMeshProUGUI text;

    public void OnSelect(BaseEventData eventData)
    {
        text.color = Color.white;
        foreach(Image background in backgrounds)
        {
            background.color = Color.black;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        text.color = Color.black;
        foreach (Image background in backgrounds)
        {
            background.color = Color.white;
        }
    }
}
