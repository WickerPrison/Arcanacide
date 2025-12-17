using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MinibossEvents : EnemyEvents
{
    public event EventHandler onThrustersOn;
    public event EventHandler onThrustersOff;
    public event EventHandler onStartPlasmaShots;
    public event EventHandler onStartDroneLaser;
    public event EventHandler onRecallDrones;
    public event EventHandler<(float, float)> onStartCircle;
    public event EventHandler onStartDroneCharge;
    public event EventHandler onFlyAway;
    public event EventHandler onTeslaHarpoons;
    public event EventHandler onDissolve;

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

    public void StartDroneLaser()
    {
        onStartDroneLaser?.Invoke(this, EventArgs.Empty);
    }

    public void RecallDrones()
    {
        onRecallDrones?.Invoke(this, EventArgs.Empty);
    }

    public void StartCircle(float startRads, float timeToCircle)
    {
        onStartCircle?.Invoke(this, (startRads, timeToCircle));
    }

    public void StartDroneCharge()
    {
        onStartDroneCharge?.Invoke(this, EventArgs.Empty);
    }

    public void FlyAway()
    {
        onFlyAway?.Invoke(this, EventArgs.Empty);
    }

    public void TeslaHarpoons()
    {
        onTeslaHarpoons?.Invoke(this, EventArgs.Empty);
    }

    public void Dissolve()
    {
        onDissolve?.Invoke(this, EventArgs.Empty);
    }
}
