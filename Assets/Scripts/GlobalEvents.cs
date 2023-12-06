using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static GlobalEvents instance { get; private set; }

    InputManager im;

    public event EventHandler onChangedSetting;
    public event EventHandler onPlayerDeath;
    public event EventHandler onEnemyKilled;
    public event EventHandler onBossKilled;
    public event EventHandler onTestButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        im = GetComponent<InputManager>();
        im.controls.Gameplay.TestButton.performed += ctx => onTestButton?.Invoke(this, EventArgs.Empty);
    }

    public void OnChangedSetting()
    {
        onChangedSetting?.Invoke(this, EventArgs.Empty);
    }

    public void OnPlayerDeath()
    {
        onPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    public void EnemyKilled()
    {
        onEnemyKilled?.Invoke(this, EventArgs.Empty);
    }

    public void BossKilled()
    {
        onBossKilled?.Invoke(this, EventArgs.Empty);
    }
}
