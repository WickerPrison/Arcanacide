using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingIndicator : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] SettingsData settingsData;
    SpriteRenderer arrow;

    private void Start()
    {
        arrow = GetComponentInChildren<SpriteRenderer>();
        arrow.enabled = settingsData.showArrow;
    }

    private void Update()
    {
        if (!settingsData.showArrow) return;

        transform.LookAt(attackPoint);
    }

    private void ChangedSetting(object sender, System.EventArgs e)
    {
        arrow.enabled = settingsData.showArrow;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onChangedSetting += ChangedSetting;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onChangedSetting -= ChangedSetting;
    }
}
