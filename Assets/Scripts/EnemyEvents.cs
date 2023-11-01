using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public event EventHandler OnTakeDamage;
    public event EventHandler OnLosePoise;
    public event EventHandler OnHitWhileInvincible;
    public event EventHandler OnAttackImpact;
    public event EventHandler OnStagger;
    public event EventHandler OnDeath;
    public event EventHandler OnStartDOT;
    public event EventHandler OnStopDOT;
    public event EventHandler OnAttack;


    public void TakeDamage()
    {
        OnTakeDamage?.Invoke(this, EventArgs.Empty);
    }

    public void LosePoise()
    {
        OnLosePoise?.Invoke(this, EventArgs.Empty);
    }

    public void HitWhileInvincible()
    {
        OnHitWhileInvincible?.Invoke(this, EventArgs.Empty);
    }

    public void AttackImpact()
    {
        OnAttackImpact?.Invoke(this, EventArgs.Empty);
    }

    public void Stagger()
    {
        OnStagger?.Invoke(this, EventArgs.Empty);
    }

    public void Death()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public void StartDOT()
    {
        OnStartDOT?.Invoke(this, EventArgs.Empty);
    }

    public void StopDOT()
    {
        OnStopDOT?.Invoke(this, EventArgs.Empty);
    }

    public void Attack()
    {
        OnAttack?.Invoke(this, EventArgs.Empty);
    }
}