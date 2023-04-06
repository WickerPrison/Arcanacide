using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public event EventHandler onPlayerStagger;

    public void PlayerStagger()
    {
        onPlayerStagger?.Invoke(this, EventArgs.Empty);
    }
}
