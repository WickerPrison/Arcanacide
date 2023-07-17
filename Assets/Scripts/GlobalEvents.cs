using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static GlobalEvents instance { get; private set; }

    public event EventHandler onEnemyKilled;
    public event EventHandler onBossKilled;

    private void Awake()
    {
        instance = this;
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
