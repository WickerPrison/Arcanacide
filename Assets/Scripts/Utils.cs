using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float GetAngle(Vector3 vectorA, Vector3 vectorB)
    {
        float angle = Mathf.Acos(Vector3.Dot(vectorA, vectorB) / (vectorA.magnitude * vectorB.magnitude));
        return angle * Mathf.Rad2Deg;
    }

    public static float GetAngle(Vector3 vector)
    {
        return GetAngle(vector, Vector3.up);
    }

    public static float GetAngle(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Mathf.Acos(Vector3.Dot(vectorA, vectorB) / (vectorA.magnitude * vectorB.magnitude));
        return angle * Mathf.Rad2Deg;
    }

    public static float GetAngle(Vector2 vector)
    {
        return GetAngle(vector, Vector2.up);
    }

    public static Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.z;
        newDirection.z = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.z;
        return newDirection;
    }

    public static void DrawDebugCircle(int pointNum, float radius, Vector3 center, float duration = 5)
    {
        float percent = 360 / pointNum;
        for (int i = 0; i < pointNum; i++)
        {
            float t = percent * i;
            Vector3 pos1 = RotateDirection(Vector3.right, t).normalized * radius + center;
            Vector3 pos2 = RotateDirection(Vector3.right, t + percent).normalized * radius + center;
            Debug.DrawLine(pos1, pos2, Color.red, duration);
        }
    }

    public static void CircleHitbox(AttackProfiles attackProfile, int attackDamage, Vector3 center, GameManager gm, PlayerAbilities playerAbilities)
    {
        Utils.DrawDebugCircle(12, attackProfile.attackRange, center, 3);
        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(enemy.transform.position, center) < attackProfile.attackRange)
            {
                playerAbilities.DamageEnemy(enemy, attackDamage, attackProfile);
            }
        }

        if (gm.enemiesInRange.Count > 0 && attackProfile.screenShakeOnHit != Vector2.zero)
        {
            GlobalEvents.instance.ScreenShake(attackProfile.screenShakeOnHit.x, attackProfile.screenShakeOnHit.y);
        }
    }

    public static List<Vector3> HitClosestXEnemies(List<EnemyScript> enemies, Vector3 origin, float range, int count, Action<EnemyScript> callback)
    {
        List<(EnemyScript, float)> enemiesWithDist = new List<(EnemyScript, float)>();
        foreach (EnemyScript enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, origin);
            if (dist <= range)
            {
                enemiesWithDist.Add((enemy, dist));
            }
        }

        List<Vector3> targets = new List<Vector3>();

        if (enemiesWithDist.Count > 0)
        {
            enemiesWithDist.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            int counter = count;
            while (counter > 0)
            {
                for (int i = 0; i < enemiesWithDist.Count; i++)
                {
                    targets.Add(enemiesWithDist[i].Item1.transform.position + Vector3.up);
                    callback(enemiesWithDist[i].Item1);
                    counter--;
                    if (counter <= 0) break;
                }
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                float x = UnityEngine.Random.Range(-1f, 1f);
                float z = UnityEngine.Random.Range(-1f, 1f);
                targets.Add(range / 2 * new Vector3(x, 0, z));
            }
        }
        return targets;
    }

    public static void IncorrectInitialization(string className)
    {
        throw new System.Exception($"{className} must be initialized with constructor method");
    }

    public static int GetWeaponIndex(PlayerWeapon weapon)
    {
        switch (weapon)
        {
            case PlayerWeapon.SWORD: return 0;
            case PlayerWeapon.LANTERN: return 1;
            case PlayerWeapon.KNIFE: return 2;
            case PlayerWeapon.CLAWS: return 3;
            default: return -1;
        }
    }

    public static int GetWeaponIndex(MenuWeaponSelected weapon)
    {
        switch (weapon)
        {
            case MenuWeaponSelected.SWORD: return 0;
            case MenuWeaponSelected.LANTERN: return 1;
            case MenuWeaponSelected.KNIFE: return 2;
            case MenuWeaponSelected.CLAWS: return 3;
            default: return -1;
        }
    }

    public static int GetElementIndex(WeaponElement element) 
    {
        switch (element)
        {
            case WeaponElement.DEFAULT: return 0;
            case WeaponElement.FIRE: return 1;
            case WeaponElement.ELECTRICITY: return 2;
            case WeaponElement.ICE: return 3;
            case WeaponElement.CHAOS: return 4;
            default: return -1;
        }
    }
}