using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleOrSwitch : MonoBehaviour
{
    ElectricPuddleScript puddleScript;
    [SerializeField] MapData mapData;
    [SerializeField] int[] powerSwitchNumber;

    // Start is called before the first frame update
    void Start()
    {
        puddleScript = GetComponent<ElectricPuddleScript>();
        puddleScript.PowerOn();
        foreach(int switchNumber in powerSwitchNumber)
        {
            if (mapData.powerSwitchesFlipped.Contains(switchNumber))
            {
                puddleScript.PowerOff();
            }
        }
    }
}
