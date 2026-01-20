using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LockOn : MonoBehaviour
{
    GameManager gm;
    InputManager im;
    bool lockOn = false;
    public EnemyScript target { get; private set; }
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        im = gm.GetComponent<InputManager>();
        im.controls.Gameplay.LockOn.performed += ctx => ToggleLockOn();
        im.controls.Gameplay.SwapTargetRight.performed += ctx => SwapTarget(true);
        im.controls.Gameplay.SwapTargetLeft.performed += ctx => SwapTarget(false);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void ToggleLockOn()
    {
        lockOn = !lockOn;
        if (lockOn)
        {
            TargetClosestEnemy();
        }
        else
        {
            target = null;
            GlobalEvents.instance.LockOnTarget(null);
        }
    }

    void TargetClosestEnemy()
    {
        target = GetClosestEnemy();
        GlobalEvents.instance.LockOnTarget(target);
    }

    EnemyScript GetClosestEnemy(float distance = 10)
    {
        float currentDistance = distance;
        EnemyScript currentTarget = null;
        for (int enemy = 0; enemy < gm.enemies.Count; enemy++)
        {
            if (Vector3.Distance(transform.position, gm.enemies[enemy].transform.position) < currentDistance && !gm.enemies[enemy].dying)
            {
                currentTarget = gm.enemies[enemy];
                currentDistance = Vector3.Distance(transform.position, gm.enemies[enemy].transform.position);
            }
        }
        return currentTarget;
    }

    public void SwapTarget(bool right)
    {
        if (gm.enemies.Count == 0)
        {
            target = null;
            GlobalEvents.instance.LockOnTarget(null);
            return;
        }
        List<(float screenPos, EnemyScript enemy)> screenEnemies = GetEnemyScreenList();
        int index = screenEnemies.FindIndex(enemy => enemy.enemy == target);
        if (right)
        {
            int targetIndex = index + 1 < screenEnemies.Count ? index + 1 : 0;
            target = screenEnemies[targetIndex].enemy;
        }
        else
        {
            int targetIndex = index > 0 ? index - 1 : screenEnemies.Count - 1;
            target = screenEnemies[targetIndex].enemy;
        }
        GlobalEvents.instance.LockOnTarget(target);
    }

    List<(float, EnemyScript)> GetEnemyScreenList()
    {
        List<(float screenPos, EnemyScript enemy)> screenEnemies = gm.enemies.Select(enemy => GetEnemyScreenSpace(enemy)).ToList();
        return screenEnemies.OrderBy(enemy => enemy.screenPos).ToList();
    }

    (float, EnemyScript) GetEnemyScreenSpace(EnemyScript enemy)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);
        return (screenPos.x, enemy);
    }

    public EnemyScript GetAbilityTarget(float distance = 10)
    {
        if (target != null) return target;

        return GetClosestEnemy(distance);
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onEnemyKilled += Global_onEnemyKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onEnemyKilled -= Global_onEnemyKilled;
    }

    private void Global_onEnemyKilled(object sender, EnemyScript enemyScript)
    {
        if(enemyScript == target)
        {
            SwapTarget(true);
        }
    }
}
