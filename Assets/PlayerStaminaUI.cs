using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUI : MonoBehaviour
{
    [SerializeField] RectMask2D staminaMask;
    [SerializeField] PlayerData playerData;
    float start = 1074;
    float spacing = 9.4f;

    private void Start()
    {
        SetStaminaFill(playerData.MaxStamina());
    }

    private void OnStaminaUpdate(GlobalEvents sender, float stamina)
    {
        SetStaminaFill(stamina);
    }

    void SetStaminaFill(float stamina)
    {
        stamina = Mathf.Ceil(stamina / 2.0175f);
        staminaMask.padding = new Vector4( 0, 0, start - spacing * stamina, 0);
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onStaminaUpdate += OnStaminaUpdate;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onStaminaUpdate -= OnStaminaUpdate;
    }
}
