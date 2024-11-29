using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaUI : MonoBehaviour
{
    [SerializeField] RectMask2D staminaMask;
    [SerializeField] RectMask2D delayMask;
    [SerializeField] PlayerData playerData;
    float start = 1074;
    float spacing = 9.4f;

    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    float delay;
    [SerializeField] float speed;
    bool buffer = false;

    private void Start()
    {
        GainStamina(playerData.MaxStamina());
    }

    private void OnGainStamina(GlobalEvents sender, float stamina)
    {
        GainStamina(stamina);
    }

    void GainStamina(float stamina)
    {
        stamina = Mathf.Ceil(stamina / 2.0175f);
        staminaMask.padding = new Vector4(0, 0, start - spacing * stamina, 0);
        delayMask.padding = staminaMask.padding;
    }

    private void OnLoseStamina(GlobalEvents sender, float stamina)
    {
        LoseStamina(stamina);
    }

    void LoseStamina(float stamina)
    {
        stamina = Mathf.Ceil(stamina / 2.0175f);
        if (!buffer)
        {
            delayMask.padding = staminaMask.padding;
            StopAllCoroutines();
            StartCoroutine(StaminaDelay());
        }
        else delay = 0.5f;
        staminaMask.padding = new Vector4( 0, 0, start - spacing * stamina, 0);
    }

    IEnumerator StaminaDelay()
    {
        delayMask.padding = staminaMask.padding;
        buffer = true;
        delay = 0.5f;
        while(delay > 0)
        {
            delay -= Time.deltaTime;
            yield return endOfFrame;
        }

        float chunk = 0;
        while(delayMask.padding.z < staminaMask.padding.z)
        {
            chunk += Time.deltaTime * speed;
            if(chunk > 1)
            {
                chunk -= 1;
                delayMask.padding += new Vector4(0, 0, spacing, 0);
                yield return endOfFrame;
            }
        }
        buffer = false;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onLoseStamina += OnLoseStamina;
        GlobalEvents.instance.onGainStamina += OnGainStamina;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onLoseStamina -= OnLoseStamina;
        GlobalEvents.instance.onGainStamina -= OnGainStamina;
    }
}
