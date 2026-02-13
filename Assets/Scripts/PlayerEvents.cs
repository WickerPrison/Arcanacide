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
    public event EventHandler<AttackProfiles> onLanternSpecial;
    public event EventHandler onLanternCombo;
    public event EventHandler<Vector3> onStartFireRain;
    public event EventHandler onEndLanternCombo;
    public event EventHandler onStartMirrorCloak;
    public event EventHandler onEndMirrorCloak;
    public event EventHandler onMeleeParry;
    public event EventHandler onSwordHeavyFullCharge;
    public event EventHandler<(Vector3, bool)> onKnifeCombo1Vfx;
    public event EventHandler<float> onWaterfowl;
    public event EventHandler onIceBreath;

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

    public void LanternSpecialAttack(AttackProfiles attackProfile)
    {
        onLanternSpecial?.Invoke(this, attackProfile);
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

    public void Waterfowl(float chargeDecimal)
    {
        onWaterfowl?.Invoke(this, chargeDecimal);
    }

    public void IceBreath()
    {
        onIceBreath?.Invoke(this, EventArgs.Empty);
    }

}
