using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackArc : AttackArcGenerator
{
    PlayerController playerController;
    GameManager gm;

    public override void Start()
    {
        base.Start();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerController = GetComponentInParent<PlayerController>();
        coneRenderer.enabled = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            gm.enemiesInRange.Add(enemy);
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

    public void ChangeArc(AttackProfiles attackProfile)
    {
        halfConeAngle = attackProfile.halfConeAngle;
        arcPoints = halfConeAngle * 2;
        DestroyImmediate(viewMesh);
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        meshFilter.mesh = viewMesh;
        colliderMesh.sharedMesh = viewMesh;
        radius = attackProfile.attackRange;
        CalculateAttackArc();
    }
}
