using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestingEvents : MonoBehaviour
{
    public event EventHandler onAttackFalse;
    public event EventHandler onFullyCharged;
    public event EventHandler onStartCharging;
    public event EventHandler onFaerieReturn;
    public event EventHandler onElectricTrapDone;
    public event EventHandler onIceBossDeathAnimation;

    public void AttackFalse()
    {
        onAttackFalse?.Invoke(this, EventArgs.Empty);
    }

    public void FullyCharged()
    {
        onFullyCharged?.Invoke(this, EventArgs.Empty);
    }

    public void StartCharging()
    {
        onStartCharging?.Invoke(this, EventArgs.Empty);
    }

    public void FaerieReturn()
    {
        onFaerieReturn?.Invoke(this, EventArgs.Empty);
    }

    public void ElectricTrapDone()
    {
        onElectricTrapDone?.Invoke(this, EventArgs.Empty);
    }

    public void IceBossDeathAnimation()
    {
        onIceBossDeathAnimation?.Invoke(this, EventArgs.Empty);
    }
}
