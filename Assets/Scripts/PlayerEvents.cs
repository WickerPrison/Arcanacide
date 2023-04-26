using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public event EventHandler onPlayerStagger;
    public event EventHandler onAxeSpecial;
    public event EventHandler onClawSpecial;
    public event EventHandler onEndClawSpecial;

    public void PlayerStagger()
    {
        onPlayerStagger?.Invoke(this, EventArgs.Empty);
    }

    public void AxeSpecialAttack()
    {
        onAxeSpecial?.Invoke(this, EventArgs.Empty);
    }

    public void ClawSpecialAttack()
    {
        onClawSpecial?.Invoke(this, EventArgs.Empty);
    }

    public void EndClawSpecialAttack()
    {
        onEndClawSpecial?.Invoke(this, EventArgs.Empty);
    }
}
