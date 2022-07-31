using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smear : MonoBehaviour
{
    [SerializeField] List<Vector3> smearPositions = new List<Vector3>();
    [SerializeField] List<Vector3> smearRotations = new List<Vector3>();
    [SerializeField] List<Vector3> smearScales = new List<Vector3>();
    ParticleSystem smear;
    ParticleSystem.ShapeModule smearShape;
    // front right = 0, front left = 1, back left = 2, back right = 3
    public int facingDirection;

    private void Start()
    {
        smear = GetComponent<ParticleSystem>();
        smearShape = smear.shape;
    }

    public void particleSmear(int smearSpeed)
    {
        smear.Clear();
        SmearDirection(smearSpeed);
        ParticleSystem.ShapeModule smearShapeFront = smear.shape;
        smearShapeFront.arcSpeed = smearSpeed;
        smear.Play();
    }

    void SmearDirection(int smearSpeed)
    {
        int smearDirection = facingDirection;
        if (smearSpeed < 0)
        {
            smearDirection += 4;
        }
        smear.transform.localScale = smearScales[facingDirection];
        smear.transform.localPosition = smearPositions[smearDirection];
        smear.transform.localRotation = Quaternion.Euler(smearRotations[smearDirection].x, smearRotations[smearDirection].y, smearRotations[smearDirection].z);
    }

    public void SpeedMultiplier(float multiplier)
    {
        smearShape.arcSpeedMultiplier = multiplier;
    }
}
