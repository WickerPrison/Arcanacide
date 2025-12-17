using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossBlade : MonoBehaviour
{
    [SerializeField] int bladeId;
    [SerializeField] Vector3 outPos;
    [SerializeField] Vector3 outRot;
    [SerializeField] Vector3 outPos2;
    [SerializeField] Vector3 outRot2;
    [SerializeField] Vector3 inPos;
    [SerializeField] Vector3 inRot;
    [SerializeField] Vector3 inPos2;
    [SerializeField] Vector3 inRot2;
    [Range(0f, 1f)]
    [SerializeField] float retraction;
    [SerializeField] Transform blade1;
    [SerializeField] Transform blade2;
    [SerializeField] bool setBladePositions;

    private void OnValidate()
    {
        if (!setBladePositions) return;
        SetRetraction();
    }

    private void OnDrawGizmosSelected()
    {
        SetRetraction();
    }

    private void Update()
    {
        SetRetraction();
    }

    void SetRetraction()
    {
        blade1.localPosition = Vector3.Lerp(inPos, outPos, retraction);
        blade1.localRotation = Quaternion.Euler(Vector3.Lerp(inRot, outRot, retraction));

        blade2.localPosition = Vector3.Lerp(inPos2, outPos2, retraction);
        blade2.localRotation = Quaternion.Euler(Vector3.Lerp(inRot2, outRot2, retraction));
    }
}
