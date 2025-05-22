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
    public event EventHandler onMinibossKilled;
    public event EventHandler onGemUsed;
    public event EventHandler onTestButton;
    public event Action<GlobalEvents, int> onPlayerMoneyChange;
    public event Action<GlobalEvents, float> onLoseStamina;
    public event Action<GlobalEvents, float> onGainStamina;
    public event Action<GlobalEvents, int> onPlayerLoseHealth;
    public event Action<GlobalEvents, int> onPlayerGainHealth;
    public event EventHandler<int> onPickupGemShard;
    public event EventHandler<bool> onEnemiesEnable;
    public event EventHandler<bool> onSwitchAC;


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

    public void MiniBossKilled()
    {
        onMinibossKilled?.Invoke(this, EventArgs.Empty);
    }

    public void GemUsed()
    {
        onGemUsed?.Invoke(this, EventArgs.Empty);
    }

    public void MoneyChange(int amount)
    {
        onPlayerMoneyChange?.Invoke(this, amount);
    }

    public void LoseStamina(float stamina)
    {
        onLoseStamina?.Invoke(this, stamina);
    }

    public void GainStamina(float stamina)
    {
        onGainStamina?.Invoke(this, stamina);
    }

    public void PlayerLoseHealth(int health)
    {
        onPlayerLoseHealth?.Invoke(this, health);
    }

    public void PlayerGainHealth(int health)
    {
        onPlayerGainHealth?.Invoke(this, health);
    }

    public void PickupGemShard()
    {
        onPickupGemShard?.Invoke(this, 1);
    }

    public void EnableEnemies(bool setActive)
    {
        onEnemiesEnable?.Invoke(this, setActive);
    }

    public void SwitchAC(bool acOn)
    {
        onSwitchAC?.Invoke(this, acOn);
    }
}
