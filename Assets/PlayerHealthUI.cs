using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] RectTransform border;
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform fill;
    [SerializeField] RectTransform fillMask;
    [SerializeField] RectTransform delayFill;
    [SerializeField] RectTransform delayFillMask;
    [SerializeField] PlayerData playerData;
    float maskDiff = 4;
    float zeroFill = 9;
    float oneHPWidth = 5f;
    bool buffer = false;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    float delay;
    [SerializeField] float speed;



    private void Start()
    {
        float borderScale = playerData.MaxHealth() * oneHPWidth - maskDiff;
        Debug.Log(borderScale);
        border.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, borderScale);
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, borderScale);
        fillMask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, borderScale - 2);
        delayFillMask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, borderScale - 2);
        GainHealth(playerData.health);
    }

    private void Instance_onPlayerGainHealth(GlobalEvents sender, int health)
    {
        GainHealth(health);
    }

    void GainHealth(int health)
    {
        fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health * oneHPWidth + zeroFill);
        delayFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health * oneHPWidth + zeroFill);
    }

    private void OnPlayerLoseHealth(GlobalEvents sender, int health)
    {
        LoseHealth(health);
    }

    void LoseHealth(int health)
    {
        if (!buffer)
        {
            StopAllCoroutines();
            StartCoroutine(HealthbarDelay());
        }
        else delay = 0.5f;
        fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health * oneHPWidth + zeroFill);
    }

    IEnumerator HealthbarDelay()
    {
        delayFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fill.rect.width);
        buffer = true;
        delay = 0.5f;
        while(delay > 0)
        {
            delay -= Time.deltaTime;
            yield return endOfFrame;
        }

        while(delayFill.rect.width > fill.rect.width)
        {
            delayFill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, delayFill.rect.width - Time.deltaTime * speed);
            yield return endOfFrame;
        }
        buffer = false;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onPlayerGainHealth += Instance_onPlayerGainHealth;
        GlobalEvents.instance.onPlayerLoseHealth += OnPlayerLoseHealth;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onPlayerGainHealth -= Instance_onPlayerGainHealth;
        GlobalEvents.instance.onPlayerLoseHealth -= OnPlayerLoseHealth;
    }
}
