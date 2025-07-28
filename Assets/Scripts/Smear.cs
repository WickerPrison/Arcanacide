using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smear : MonoBehaviour
{
    [SerializeField] List<Vector3> smearPositions = new List<Vector3>();
    [SerializeField] List<Vector3> smearRotations = new List<Vector3>();
    [SerializeField] List<Vector3> smearScales = new List<Vector3>();
    [SerializeField] bool expandedScaleList = false;

    [SerializeField] List<Vector3> alternateSmearPositions = new List<Vector3>();
    [SerializeField] List<Vector3> alternateSmearRotations = new List<Vector3>();
    [SerializeField] List<Vector3> alternateSmearScales = new List<Vector3>();
    ParticleSystem smear;
    ParticleSystem.ShapeModule smearShape;
    // front right = 0, front left = 1, back left = 2, back right = 3
    [System.NonSerialized] public int facingDirection;

    private void Start()
    {
        smear = GetComponent<ParticleSystem>();
        smearShape = smear.shape;
        smear.Simulate(1);
    }

    public void particleSmear(int smearSpeed)
    {
        smear.Clear();
        SmearDirection(smearSpeed);
        ParticleSystem.ShapeModule smearShapeFront = smear.shape;
        smearShapeFront.arcSpeed = smearSpeed;
        smear.Simulate(0, false);
    }

    void SmearDirection(int smearSpeed)
    {
        int smearDirection = facingDirection;
        if (smearSpeed < 0)
        {
            smearDirection += 4;
        }
        smear.transform.localScale  = expandedScaleList ? smearScales[smearDirection] : smearScales[facingDirection];
        smear.transform.localPosition = smearPositions[smearDirection];
        smear.transform.localRotation = Quaternion.Euler(smearRotations[smearDirection].x, smearRotations[smearDirection].y, smearRotations[smearDirection].z);
    }

    public void AlternateSmears(int smearSpeed, int alternateIndex)
    {
        smear.Clear();
        AlternateDirection(alternateIndex);
        ParticleSystem.ShapeModule smearShapeFront = smear.shape;
        smearShapeFront.arcSpeed = smearSpeed;
        smear.Simulate(0, false);
    }

    void AlternateDirection(int alternateIndex)
    {
        int smearDirection = facingDirection;
        smearDirection += 4 * alternateIndex;
        smear.transform.localScale = alternateSmearScales[smearDirection];
        smear.transform.localPosition = alternateSmearPositions[smearDirection];
        smear.transform.localRotation = Quaternion.Euler(alternateSmearRotations[smearDirection].x, alternateSmearRotations[smearDirection].y, alternateSmearRotations[smearDirection].z);
    }

    public void SpeedMultiplier(float multiplier)
    {
        smearShape.arcSpeedMultiplier = multiplier;
    }

    private void FixedUpdate()
    {
        smear.Simulate(Time.fixedDeltaTime, false, false);
    }
}
