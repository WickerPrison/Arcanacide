using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public event EventHandler onDashStart;
    public event EventHandler onDashEnd;
    public event EventHandler onBackstepStart;
    [System.NonSerialized] public int backstepInt;
    public event EventHandler onAttackImpact;
    public event EventHandler onTakeDamage;
    public event EventHandler onPlayerStagger;
    public event EventHandler onEndPlayerStagger;
    public event EventHandler onAxeSpecial;
    public event EventHandler onLanternCombo;
    public event EventHandler<Vector3> onStartFireRain;
    public event EventHandler onEndLanternCombo;
    public event EventHandler onClawSpecial;
    public event EventHandler onEndClawSpecial;
    public event EventHandler onStartMirrorCloak;
    public event EventHandler onEndMirrorCloak;
    public event EventHandler onMeleeParry;
    public event EventHandler onSwordHeavyFullCharge;
    public event EventHandler<(Vector3, bool)> onKnifeCombo1Vfx;

    public void DashStart()
    {
        onDashStart?.Invoke(this, EventArgs.Empty);
    }

    public void DashEnd()
    {
        onDashEnd?.Invoke(this, EventArgs.Empty);
    }

    public void BackstepStart(int num)
    {
        backstepInt = num;
        onBackstepStart?.Invoke(this, EventArgs.Empty);
    }

    public void AttackImpact()
    {
        onAttackImpact?.Invoke(this, EventArgs.Empty);
    }

    public void TakeDamage()
    {
        onTakeDamage?.Invoke(this, EventArgs.Empty);
    }

    public void PlayerStagger()
    {
        onPlayerStagger?.Invoke(this, EventArgs.Empty);
    }

    public void EndPlayerStagger()
    {
        onEndPlayerStagger?.Invoke(this, EventArgs.Empty);
    }

    public void AxeSpecialAttack()
    {
        onAxeSpecial?.Invoke(this, EventArgs.Empty);
    }

    public void LanternCombo()
    {
        onLanternCombo?.Invoke(this, EventArgs.Empty);
    }

    public void StartFireRain(Vector3 origin)
    {
        onStartFireRain?.Invoke(this, origin);
    }

    public void EndLanternCombo()
    {
        onEndLanternCombo?.Invoke(this, EventArgs.Empty);
    }

    public void ClawSpecialAttack()
    {
        onClawSpecial?.Invoke(this, EventArgs.Empty);
    }

    public void EndClawSpecialAttack()
    {
        onEndClawSpecial?.Invoke(this, EventArgs.Empty);
    }

    public void StartMirrorCloak()
    {
        onStartMirrorCloak?.Invoke(this, EventArgs.Empty);
    }

    public void EndMirrorCloak()
    {
        onEndMirrorCloak?.Invoke(this, EventArgs.Empty);
    }

    public void MeleeParry()
    {
        onMeleeParry?.Invoke(this, EventArgs.Empty);
    }

    public void SwordHeavyFullCharge()
    {
        onSwordHeavyFullCharge?.Invoke(this, EventArgs.Empty);
    }

    public void KnifeCombo1Vfx(Vector3 direction, bool front)
    {
        onKnifeCombo1Vfx?.Invoke(this, (direction, front));
    }
}
