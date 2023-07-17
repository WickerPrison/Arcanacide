using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpBackButton : MonoBehaviour, ISelectHandler
{
    LevelUpMenu menu;

    private void Awake()
    {
        menu = GetComponentInParent<LevelUpMenu>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        menu.HideDescription();
    }
}
