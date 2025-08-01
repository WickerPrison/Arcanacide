using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemoteACUI : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] TextMeshProUGUI thermostatDisplay;
    [SerializeField] GameObject thermostat;
    [SerializeField] TextMeshProUGUI prompt;

    public void ShowThermostat(bool show)
    {
        if (show)
        {
            thermostat.SetActive(true);
            SwitchAC(mapData.ACOn);
        }
        else
        {
            thermostat.SetActive(false);
        }
    }

    void SwitchAC(bool acOn)
    {
        thermostatDisplay.text = acOn ? "MAX" : "OFF";
    }

    private void OnEnable()
    {
        ShowThermostat(mapData.hasRemoteAC);
        GlobalEvents.instance.onSwitchAC += Global_onSwitchAC;
        GlobalEvents.instance.onAwareEnemiesChange += Global_onAwareEnemiesChange;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onSwitchAC -= Global_onSwitchAC;
        GlobalEvents.instance.onAwareEnemiesChange -= Global_onAwareEnemiesChange;
    }

    private void Global_onSwitchAC(object sender, bool acOn)
    {
        SwitchAC(acOn);
    }

    private void Global_onAwareEnemiesChange(object sender, int count)
    {
        prompt.gameObject.SetActive(count <= 0);
    }
}
