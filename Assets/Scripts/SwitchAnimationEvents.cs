using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimationEvents : MonoBehaviour
{
    public void PowerOff()
    {
        PowerSwitch switchScript = GetComponentInParent<PowerSwitch>();
        switchScript.PowerOff();
    }
}
