using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MinibossAbilities : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] Transform missileSpawnPoint;
    EnemyController enemyController;
    EnemyScript enemyScript;
    PlayerScript playerScript;
    float range = 3.5f;
    float spread = 3.5f;
    WaitForSeconds salvoDelay = new WaitForSeconds(0.3f);
    public ObjectPool<GameObject> pool;


    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyScript = GetComponent<EnemyScript>();
        playerScript = enemyController.playerScript;
    }

    public void MissileAttack()
    {
        enemyController.state = EnemyState.ATTACKING;
        enemyController.frontAnimator.Play("Missiles");
        enemyController.backAnimator.Play("Missiles");
    }

    public void SingleMissile(Vector3 target, float timeToHit)
    {
        ArcProjectile missile = Instantiate(missilePrefab).GetComponent<ArcProjectile>();
        missile.transform.position = missileSpawnPoint.position;
        missile.endPoint = target;
        missile.enemyOfOrigin = enemyScript;
        missile.timeToHit = timeToHit;
    }

    public void FireMissiles()
    {
        StartCoroutine(MissileCoroutine());
    }

    IEnumerator MissileCoroutine()
    {
        Vector3 playerDirection = Vector3.Normalize(playerScript.transform.position - transform.position);
        for (int i = 1; i < 4; i++)
        {
            Vector3 forwardPosition = transform.position + playerDirection * i * range;
            for (int j = -2; j <= 2; j++)
            {
                Vector3 target = forwardPosition + Vector3.Cross(playerDirection, Vector3.up).normalized * j * spread;
                target += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                SingleMissile(target, 0.5f + (float)Mathf.Abs(j) / 4);
            }
            yield return salvoDelay;
        }
    }
}
