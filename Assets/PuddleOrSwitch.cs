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
        if (!mapData.powerSwitchesFlipped.Contains(powerSwitchNumber[0]) && !mapData.powerSwitchesFlipped.Contains(powerSwitchNumber[1]))
        {
            puddleScript.PowerOn();
        }
    }
}
