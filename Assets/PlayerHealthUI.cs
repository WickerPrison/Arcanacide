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
    [SerializeField] RectMask2D delayMask;
    [SerializeField] PlayerData playerData;
    float maxWidth = 1354;
    float maskDiff = 4;
    float maxFillWidth = 1350;
    float zeroFill = 9;
    float healthRatio = 6f;

    private void Start()
    {
        float borderScale = playerData.MaxHealth() * healthRatio - maskDiff;
        border.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, borderScale);
        background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, borderScale);
        fillMask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, borderScale - 2);
        GainHealth(playerData.health);
    }

    private void Instance_onPlayerGainHealth(GlobalEvents sender, int health)
    {
        GainHealth(health);
    }

    void GainHealth(int health)
    {
        fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health * healthRatio + zeroFill);
    }

    private void OnPlayerLoseHealth(GlobalEvents sender, int health)
    {
        LoseHealth(health);
    }

    void LoseHealth(int health)
    {
        fill.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health * healthRatio + zeroFill);
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
