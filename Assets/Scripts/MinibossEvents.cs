using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinibossEvents : EnemyEvents
{
    public event EventHandler onThrustersOn;
    public event EventHandler onThrustersOff;
    public event EventHandler onStartPlasmaShots;

    public void ThrustersOn()
    {
        onThrustersOn?.Invoke(this, EventArgs.Empty);
    }

    public void ThrustersOff()
    {
        onThrustersOff?.Invoke(this, EventArgs.Empty);
    }

    public void StartPlasmaShots()
    {
        onStartPlasmaShots?.Invoke(this, EventArgs.Empty);
    }

}
