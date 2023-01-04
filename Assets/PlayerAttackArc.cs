using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttackArc : AttackArcGenerator
{
    PlayerController playerController;

    public override void Start()
    {
        base.Start();
        playerController = GetComponentInParent<PlayerController>();
        coneRenderer.enabled = true;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            playerController.enemiesInRange.Add(other);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            playerController.enemiesInRange.Remove(other);
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
        radius = attackProfile.attackArcRadius;
        CalculateAttackArc();
    }
}
