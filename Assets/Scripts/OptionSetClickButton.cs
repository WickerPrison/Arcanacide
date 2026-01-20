using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionSetClickButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] bool isRightButton;
    [SerializeField] OptionSetType type;
    SettingsMenu settingsMenu;

    enum OptionSetType
    {
        FRAME_RATE_LIMIT
    }

    private void Start()
    {
        settingsMenu = GetComponentInParent<SettingsMenu>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch(type, isRightButton)
        {
            case (OptionSetType.FRAME_RATE_LIMIT, true):
                settingsMenu.FrameRateRight();
                break;
            case (OptionSetType.FRAME_RATE_LIMIT, false):
                settingsMenu.FrameRateLeft();
                break;
        }
    }
}
