using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinibossEvents : EnemyEvents
{
    public event EventHandler onThrustersOn;
    public event EventHandler onThrustersOff;

    public void ThrustersOn()
    {
        onThrustersOn?.Invoke(this, EventArgs.Empty);
    }

    public void ThrustersOff()
    {
        onThrustersOff?.Invoke(this, EventArgs.Empty);
    }
}
