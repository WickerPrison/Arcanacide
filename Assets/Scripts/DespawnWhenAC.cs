using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnWhenAC : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] bool despawnWhenOn;

    private void Start()
    {
        if (mapData.ACOn == despawnWhenOn)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onSwitchAC += Global_onSwitchAC;
    }

    private void Global_onSwitchAC(object sender, bool acOn)
    {
        if(acOn == despawnWhenOn)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
