using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public int maxHealth;
    public int health;
    public float maxStamina;
    public int attackPower;
    public string equippedAbility;
    public float duckCD;
    public bool hasHealed;
    public bool hasSpawned;
    public int lastAltar;
    public bool hasBlock;
    public int money;
    public int lostMoney;

    private void OnEnable()
    {
        health = maxHealth;
        equippedAbility = "Heal";
        hasHealed = false;
        hasSpawned = false;
    }
}
