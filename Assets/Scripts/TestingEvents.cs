using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestingEvents : MonoBehaviour
{
    public event EventHandler onAttackFalse;
    public event EventHandler onFullyCharged;
    public event EventHandler onFaerieReturn;
    public event EventHandler onElectricTrapDone;

    public void AttackFalse()
    {
        onAttackFalse?.Invoke(this, EventArgs.Empty);
    }

    public void FullyCharged()
    {
        onFullyCharged?.Invoke(this, EventArgs.Empty);
    }

    public void FaerieReturn()
    {
        onFaerieReturn?.Invoke(this, EventArgs.Empty);
    }

    public void ElectricTrapDone()
    {
        onElectricTrapDone?.Invoke(this, EventArgs.Empty);
    }
}
