using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    TextMeshProUGUI buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonText.color = Color.white;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        buttonText.color = Color.black;
    }
}
