using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] PlayerStats playerStats;

    private void onEnemyKilled(object sender, EnemyScript enemyScript)
    {
        playerStats.killedEnemies++;
        switch (playerStats.killedEnemies)
        {
            case 5:
                dialogueData.smackGPTQueue.Add(1);
                break;
            case 15:
                dialogueData.smackGPTQueue.Add(2);
                break;
        }

        if (playerData.unlockedAbilities.Contains(UnlockableAbilities.BLOCK) && playerStats.killedEnemies - playerStats.killedEnemiesAtGainBlock == 6)
        {
            dialogueData.smackGPTQueue.Add(3);
        }

        if(enemyScript.enemyType != EnemyType.UNDECLARED)
        {
            switch (enemyScript.enemyType)
            {
                case EnemyType.FIRE_BOSS:
                    dialogueData.smackGPTQueue.Remove(4);
                    break;
                case EnemyType.ELECTRIC_BOSS:
                    dialogueData.smackGPTQueue.Remove(5);
                    break;
                case EnemyType.ICE_BOSS:
                    dialogueData.smackGPTQueue.Remove(6);
                    break;
            }
        }
    }

    private void Global_onGainBlock(object sender, EventArgs e)
    {
        playerStats.killedEnemiesAtGainBlock = playerStats.killedEnemies;
    }

    private void Global_onPlayerDeath(object sender, EnemyScript killedBy)
    {
        playerStats.totalDeaths++;
        switch (playerStats.totalDeaths)
        {
            case 1:
                dialogueData.directorQueue.Add(1);
                break;
        }

        if(killedBy != null && killedBy.enemyType != EnemyType.UNDECLARED)
        {
            int count = playerStats.IncrementDeathsToEnemy(killedBy.enemyType);
            switch((killedBy.enemyType, count))
            {
                case (EnemyType.FIRE_BOSS, 7):
                    dialogueData.smackGPTQueue.Add(4);
                    break;
                case (EnemyType.ELECTRIC_BOSS, 7):
                    if(mapData.carolsDeadFriends.Count < 3)
                    {
                        dialogueData.smackGPTQueue.Add(5);
                    }
                    break;
                case (EnemyType.ICE_BOSS, 7):
                    dialogueData.smackGPTQueue.Add(6);
                    break;
            }
        }
    }

    private void Global_onGainWeapon(object sender, EventArgs e)
    {
        int unlockedWeaponsCount = playerData.unlockedSwords.Count + playerData.unlockedLanterns.Count + playerData.unlockedKnives.Count + playerData.unlockedClaws.Count;
        playerStats.UnlockedWeapons(unlockedWeaponsCount);
        if (!dialogueData.smackGPTPreviousConversations.Contains(7)) dialogueData.smackGPTQueue.Add(7);
    }

    private void Global_onGainWeaponOfSameType(object sender, int weaponId)
    {
        if (dialogueData.smackGPTPreviousConversations.Contains(8)) return;
        if(playerData.GetWeaponUnlockList(weaponId).Count < 2) return;
        dialogueData.smackGPTQueue.Add(8);
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onEnemyKilled += onEnemyKilled;
        GlobalEvents.instance.onGainBlock += Global_onGainBlock;
        GlobalEvents.instance.onPlayerDeath += Global_onPlayerDeath;
        GlobalEvents.instance.onGainWeapon += Global_onGainWeapon;
        GlobalEvents.instance.onGainWeaponOfSameType += Global_onGainWeaponOfSameType;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onEnemyKilled -= onEnemyKilled;
        GlobalEvents.instance.onGainBlock -= Global_onGainBlock;
        GlobalEvents.instance.onPlayerDeath -= Global_onPlayerDeath;
        GlobalEvents.instance.onGainWeapon -= Global_onGainWeapon;
        GlobalEvents.instance.onGainWeaponOfSameType -= Global_onGainWeaponOfSameType;
    }
}
