using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpMenuBufferRegion : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] LevelUpMenu levelUpMenu;

    public void OnPointerClick(PointerEventData eventData)
    {
        levelUpMenu.IncreaseSelectedStat();
    }
}
