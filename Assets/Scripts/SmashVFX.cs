using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashVFX : MonoBehaviour
{
    [SerializeField] ParticleSystem impactLines;
    [SerializeField] ParticleSystem dustClouds;
    EnemyEvents enemyEvents;
    AttackArcGenerator attackArcGenerator;
    ParticleSystem.ShapeModule impactLinesShape;
    ParticleSystem.ShapeModule dustCloudsShape;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    private void Start()
    {
        attackArcGenerator = GetComponentInParent<AttackArcGenerator>();
        impactLinesShape = impactLines.shape;
        dustCloudsShape = dustClouds.shape;
    }

    private void OnAttack(object sender, System.EventArgs e)
    {
        impactLinesShape.angle = attackArcGenerator.halfConeAngle;
        dustCloudsShape.angle = attackArcGenerator.halfConeAngle;
        impactLines.Play();
        dustClouds.Play();
    }

    private void OnEnable()
    {
        enemyEvents.OnAttack += OnAttack;
    }

    private void OnDisable()
    {
        enemyEvents.OnAttack -= OnAttack;
    }
}
