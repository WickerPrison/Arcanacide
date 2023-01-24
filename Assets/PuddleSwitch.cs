using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleSwitch : MonoBehaviour
{
    ElectricPuddleScript puddleScript;
    [SerializeField] MapData mapData;
    [SerializeField] int powerSwitchNumber;
    bool puddleOn = false;

    // Start is called before the first frame update
    void Start()
    {
        puddleScript = GetComponent<ElectricPuddleScript>();
        if (!mapData.powerSwitchesFlipped.Contains(powerSwitchNumber))
        {
            puddleScript.PowerOn();
            puddleOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (puddleOn && mapData.powerSwitchesFlipped.Contains(powerSwitchNumber))
        {
            puddleScript.PowerOff();
        }
    }
}
