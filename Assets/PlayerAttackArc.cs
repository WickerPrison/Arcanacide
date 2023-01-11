using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackArc : AttackArcGenerator
{
    PlayerController playerController;
    GameManager gm;
    LayerMask enemiesLayerMask;

    public override void Start()
    {
        base.Start();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerController = GetComponentInParent<PlayerController>();
        coneRenderer.enabled = true;
        enemiesLayerMask = LayerMask.GetMask("Enemy");
    }

    /*
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            if (!gm.enemiesInRange.Contains(enemy))
            {
                gm.enemiesInRange.Add(enemy);
            }
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            gm.enemiesInRange.Remove(enemy);
        }
    }
    */

    public void ChangeArc(AttackProfiles attackProfile)
    {
        halfConeAngle = attackProfile.halfConeAngle;
        arcPoints = halfConeAngle * 2;
        DestroyImmediate(viewMesh);
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        meshFilter.mesh = viewMesh;
        radius = attackProfile.attackRange;
        CalculateAttackArc();
    }

    public void GetEnemiesInRange()
    {
        gm.enemiesInRange.Clear();

        for (int i = 0; i < arcPoints; i += 5)
        {
            float angle = i - halfConeAngle;
            float angleToZero = Mathf.Acos(Vector3.Dot(Vector3.forward, transform.forward) / (Vector3.forward.magnitude * transform.forward.magnitude));
            if (transform.forward.x >= 0)
            {
                angle += angleToZero * Mathf.Rad2Deg;
            }
            else
            {
                angle -= angleToZero * Mathf.Rad2Deg;
            }
            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction.normalized, out hit, radius, enemiesLayerMask, QueryTriggerInteraction.Ignore))
            {
                EnemyScript enemyScript = hit.collider.gameObject.GetComponent<EnemyScript>();
                if (!gm.enemiesInRange.Contains(enemyScript))
                {
                    gm.enemiesInRange.Add(enemyScript);
                }
            }
            Debug.DrawRay(transform.position, direction.normalized * radius, Color.red);
        }
    } 
}
