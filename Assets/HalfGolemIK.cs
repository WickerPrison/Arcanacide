using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class HalfGolemIK : MonoBehaviour
{
    [SerializeField] int limbID;
    HalfGolemController controller;
    LimbSolver2D limbSolver;

    private void Awake()
    {
        controller = GetComponentInParent<HalfGolemController>();
        limbSolver = GetComponent<LimbSolver2D>();
    }

    private void onIceBreak(object sender, EventArgs e)
    {
        if(limbID == controller.remainingIce)
        {
            limbSolver.weight = 1;
        }
    }

    private void OnEnable()
    {
        controller.onIceBreak += onIceBreak;
    }

    private void OnDisable()
    {
        controller.onIceBreak -= onIceBreak;
    }
}
