using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
    PowerGenerator[] generators;
    [SerializeField] int switchNumber;
    [SerializeField] MapData mapData;
    bool isOn = true;

    private void Start()
    {
        generators = GetComponentsInChildren<PowerGenerator>();
        if (mapData.powerSwitchesFlipped.Contains(switchNumber))
        {
            isOn = false;
            foreach (PowerGenerator generator in generators)
            {
                generator.TurnOff();
            }
        }
    }

    private void Update()
    {
        if (isOn && mapData.powerSwitchesFlipped.Contains(switchNumber))
        {
            isOn = false;
            foreach (PowerGenerator generator in generators)
            {
                generator.TurnOff();
            }
        }
    }
}
