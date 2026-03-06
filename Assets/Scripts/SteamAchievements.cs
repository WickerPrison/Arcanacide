using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public static class SteamAchievements
{
    static Dictionary<EnemyType, string> enemyTypeToApiKey = new Dictionary<EnemyType, string>()
    {
        { EnemyType.FIRE_BOSS, "fireBossDeaths" },
        { EnemyType.ELECTRIC_BOSS, "carolDeaths" },
        { EnemyType.ICE_BOSS, "frankDeaths" },
        { EnemyType.CHAOS_BOSS, "ceoDeaths" },
        { EnemyType.MINIBOSS_V1, "mbv1Deaths" },
        { EnemyType.MINIBOSS_V2, "mbv2Deaths" },
        { EnemyType.MINIBOSS_V3, "mbv3Deaths" },
        { EnemyType.MINIBOSS_V4, "mbv4Deaths" },
    };

    static Dictionary<EvidenceFloor, string> evidenceFloorToApiKey = new Dictionary<EvidenceFloor, string>()
    {
        { EvidenceFloor.FIRST, "evidence1" }, 
        { EvidenceFloor.SECOND, "evidence2" }, 
        { EvidenceFloor.THIRD, "evidence3" }, 
    };

    public static void UpdateDeaths(int value)
    {
        UpdateStat("deaths", value);
    }

    public static void UpdateDeathsToEnemy(EnemyType enemyType, int value)
    {
        UpdateStat(enemyTypeToApiKey[enemyType], value);
    }

    public static void UpdateFloorEvidence(EvidenceFloor evidenceFloor, int value)
    {
        UpdateStat(evidenceFloorToApiKey[evidenceFloor], value);
    }

    public static void UpdateTotalEvidence(int value)
    {
        UpdateStat("totalEvidence", value);
    }

    public static void UpdateWeaponsUnlocked(int value)
    {
        UpdateStat("weaponsUnlocked", value);
    }

    public static void UpdateRefundStoneUpgrades(int value)
    {
        UpdateStat("refundUpgrades", value);
    }

    static void UpdateStat(string apiKey, int value)
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetStat(apiKey, value);
        SteamUserStats.StoreStats();
        SteamUserStats.GetStat(apiKey, out int stat);
        Debug.Log($"Steam stat for {apiKey} is {stat}");
    }
}
