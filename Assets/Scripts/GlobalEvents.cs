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
    public event EventHandler<EnemyScript> onEnemyKilled;
    public event EventHandler onBossKilled;
    public event EventHandler onMinibossEndDialogue;
    public event EventHandler onMinibossKilled;
    public event EventHandler onWhistleblowerKilled;
    public event EventHandler onGemUsed;
    public event EventHandler onTestButton;
    public event Action<GlobalEvents, (int, int)> onPlayerMoneyChange;
    public event Action<GlobalEvents, float> onLoseStamina;
    public event Action<GlobalEvents, float> onGainStamina;
    public event Action<GlobalEvents, int> onPlayerLoseHealth;
    public event Action<GlobalEvents, int> onPlayerGainHealth;
    public event EventHandler<int> onPickupGemShard;
    public event EventHandler<bool> onEnemiesEnable;
    public event EventHandler<bool> onSwitchAC;
    public event EventHandler onACWallSwitch;
    public event EventHandler<(float, float)> onScreenShake;
    public event EventHandler<int> onAwareEnemiesChange;
    public event EventHandler<int> onChangeWeapon;
    public event EventHandler<int> onPlayerDealDamage;
    public event EventHandler onPlayerStatsChange;
    public event EventHandler<EnemyScript> onLockOnTarget;


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

    public void EnemyKilled(EnemyScript enemyScript)
    {
        onEnemyKilled?.Invoke(this, enemyScript);
    }

    public void BossKilled()
    {
        onBossKilled?.Invoke(this, EventArgs.Empty);
    }

    public void MinibossEndDialogue()
    {
        onMinibossEndDialogue?.Invoke(this, EventArgs.Empty);
    }

    public void MiniBossKilled()
    {
        onMinibossKilled?.Invoke(this, EventArgs.Empty);
    }

    public void WhistleblowerKilled()
    {
        onWhistleblowerKilled?.Invoke(this, EventArgs.Empty);
    }

    public void GemUsed()
    {
        onGemUsed?.Invoke(this, EventArgs.Empty);
    }

    public void MoneyChange(int oldValue, int change)
    {
        onPlayerMoneyChange?.Invoke(this, (oldValue, change));
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

    public void ACWallSwitch()
    {
        onACWallSwitch?.Invoke(this, EventArgs.Empty);
    }

    public void ScreenShake(Vector2 screenShake)
    {
        ScreenShake(screenShake.x, screenShake.y);
    }

    public void ScreenShake(float duration, float magnitude)
    {
        onScreenShake?.Invoke(this, (duration, magnitude));
    }

    public void AwareEnemiesChange(int count)
    {
        onAwareEnemiesChange?.Invoke(this, count);
    }

    public void ChangeWeapon(int index)
    {
        onChangeWeapon?.Invoke(this, index);
    }

    public void PlayerDealDamage(int damage)
    {
        onPlayerDealDamage?.Invoke(this, damage);
    }

    public void PlayerStatsChange()
    {
        onPlayerStatsChange?.Invoke(this, EventArgs.Empty);
    }

    public void LockOnTarget(EnemyScript target)
    {
        onLockOnTarget?.Invoke(this, target);
    }
}
