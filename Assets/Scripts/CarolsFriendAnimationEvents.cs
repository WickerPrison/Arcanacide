using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarolsFriendAnimationEvents : MonoBehaviour
{
    [SerializeField] LightningHalo lightningHalo;
    public void EndLightningHalo()
    {
        lightningHalo.ShowRings(false);
    }
}
