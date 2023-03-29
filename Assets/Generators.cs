using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generators : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] int switchNumber;
    PowerGenerator[] generators;
    bool isOn = true;

    private void Start()
    {
        generators = GetComponentsInChildren<PowerGenerator>();
        if (mapData.powerSwitchesFlipped.Contains(switchNumber))
        {
            isOn = false;
            foreach(PowerGenerator powerGenerator in generators)
            {
                powerGenerator.PowerOff();
            }
        }
    }

    private void Update()
    {
        if (!isOn) return;

        if (mapData.powerSwitchesFlipped.Contains(switchNumber))
        {
            isOn = false;
            foreach (PowerGenerator powerGenerator in generators)
            {
                powerGenerator.PowerOff();
            }
        }
    }
}
