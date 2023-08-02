using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static GlobalEvents instance { get; private set; }

    InputManager im;

    public event EventHandler onEnemyKilled;
    public event EventHandler onBossKilled;
    public event EventHandler onTestButton;
    public event EventHandler onTicketFiled;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        im = GetComponent<InputManager>();
        im.controls.Gameplay.TestButton.performed += ctx => onTestButton?.Invoke(this, EventArgs.Empty);
    }

    public void EnemyKilled()
    {
        onEnemyKilled?.Invoke(this, EventArgs.Empty);
    }

    public void BossKilled()
    {
        onBossKilled?.Invoke(this, EventArgs.Empty);
    }

    public void TicketFiled()
    {
        onTicketFiled?.Invoke(this, EventArgs.Empty);
    }
}
