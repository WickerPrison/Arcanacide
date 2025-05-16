using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHaveLightningRings
{
    public event EventHandler<float> onSetRadius;
    public event EventHandler<Transform> onSetTarget;
    public event EventHandler<bool> onShowRings;
}
