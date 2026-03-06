using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

//These are all the steam achievements that are not triggered based on a stat
public enum Achievement
{
    KILL_DAVE, KILL_CAROL, KILL_FRANK, KILL_CEO, KILL_V1, KILL_V2, KILL_V3, KILL_V4, AGENT_FREI, KILL_IT_WORKER
}

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

    static Dictionary<Achievement, string> achievementToApiKey = new Dictionary<Achievement, string>()
    {
        { Achievement.KILL_DAVE, "killDave" },
        { Achievement.KILL_CAROL, "killCarol" },
        { Achievement.KILL_FRANK, "killFrank" },
        { Achievement.KILL_CEO, "killCEO" },
        { Achievement.KILL_V1, "minibossV1" },
        { Achievement.KILL_V2, "minibossV2" },
        { Achievement.KILL_V3, "minibossV3" },
        { Achievement.KILL_V4, "minibossV4" },
        { Achievement.AGENT_FREI, "agentFrei" },
        { Achievement.KILL_IT_WORKER, "KillItWorker" },
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

    public static void UnlockAchievement(Achievement achievement)
    {
        if (!SteamManager.Initialized) return;
        SteamUserStats.SetAchievement(achievementToApiKey[achievement]);
        SteamUserStats.StoreStats();
        SteamUserStats.GetAchievement(achievementToApiKey[achievement], out bool achieved);
        Debug.Log($"Steam achievement {achievement} achieved: {achieved}");
    }
}
