using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestingEvents : MonoBehaviour
{
    public event EventHandler onAttackFalse;

    public void AttackFalse()
    {
        onAttackFalse?.Invoke(this, EventArgs.Empty);
    }
}
