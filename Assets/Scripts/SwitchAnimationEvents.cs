using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimationEvents : MonoBehaviour
{
    public void FlipOff()
    {
        IFlipOff switchScript = GetComponentInParent<IFlipOff>();
        if(switchScript != null)
        {
            switchScript.FlipOff();
        }
    }

    public void FlipOn()
    {
        IFlipOn switchScript = GetComponentInParent<IFlipOn>();
        if(switchScript != null)
        {
            switchScript.FlipOn();
        }
    }
}
