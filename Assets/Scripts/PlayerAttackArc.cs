using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackArc : AttackArcGenerator
{
    GameManager gm;
    LayerMask enemiesLayerMask;
    Vector3 forwardVector;

    public override void Start()
    {
        base.Start();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //coneRenderer.enabled = true;
        enemiesLayerMask = LayerMask.GetMask("Enemy");
        leftIndex = 6;
        rightIndex = 6;
        forwardVector = transform.forward;
        angleForward = Vector3.Angle(forwardVector, vertices[0]);
        angleLeftSide = Vector3.Angle(forwardVector, vertices[leftIndex] - vertices[0]);
        angleRightSide = Vector3.Angle(forwardVector, vertices[arcPoints - rightIndex] - vertices[arcPoints]);
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
        RaycastHit hit;

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
            //Debug.DrawRay(transform.position, direction.normalized * radius, Color.red, 5f);
            if (Physics.Raycast(transform.position, direction.normalized, out hit, radius, enemiesLayerMask, QueryTriggerInteraction.Ignore))
            {
                AddToEnemiesInRange(hit);
            }
        }

        Vector3 baseDirection = Quaternion.AngleAxis(-angleForward, Vector3.up) * transform.forward;
        float leftSideDistance = Vector3.Distance(vertices[0], vertices[leftIndex]);
        Vector3 leftSideDirection = Quaternion.AngleAxis(-angleLeftSide, Vector3.up) * transform.forward;

        //Debug.DrawRay(transform.position + baseDirection * vertices[0].magnitude, leftSideDirection.normalized * leftSideDistance, Color.red, 5f);
        if(Physics.Raycast(transform.position + baseDirection * vertices[0].magnitude, leftSideDirection.normalized, out hit, leftSideDistance, enemiesLayerMask, QueryTriggerInteraction.Ignore))
        {
            AddToEnemiesInRange(hit);
        }

        baseDirection = Quaternion.AngleAxis(angleForward, Vector3.up) * transform.forward;
        float rightSideDistance = Vector3.Distance(vertices[arcPoints], vertices[arcPoints - rightIndex]);
        Vector3 rightSideDirection = Quaternion.AngleAxis(angleRightSide, Vector3.up) * transform.forward;

        //Debug.DrawRay(transform.position + baseDirection * vertices[arcPoints].magnitude, rightSideDirection.normalized * rightSideDistance, Color.red, 5f);
        if (Physics.Raycast(transform.position + baseDirection * vertices[arcPoints].magnitude, rightSideDirection.normalized, out hit, rightSideDistance, enemiesLayerMask, QueryTriggerInteraction.Ignore))
        {
            AddToEnemiesInRange(hit);
        }

        //Debug.DrawRay(transform.parent.position, transform.forward * radius, Color.red, 5f);
        if(Physics.Raycast(transform.parent.position, transform.forward, out hit, radius, enemiesLayerMask, QueryTriggerInteraction.Ignore))
        {
            AddToEnemiesInRange(hit);
        }
    } 

    void AddToEnemiesInRange(RaycastHit hit)
    {
        EnemyScript enemyScript = hit.collider.gameObject.GetComponent<EnemyScript>();
        if (!gm.enemiesInRange.Contains(enemyScript))
        {
            gm.enemiesInRange.Add(enemyScript);
        }
    }
}
