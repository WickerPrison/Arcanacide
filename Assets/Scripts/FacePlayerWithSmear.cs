using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FacePlayerWithSmear : FacePlayer
{
    Smear smear;

    public override void Start()
    {
        base.Start();
        smear = GetComponentInChildren<Smear>();
    }

    public override void FrontRight()
    {
        base.FrontRight();
        smear.facingDirection = 0;
    }

    public override void FrontLeft()
    {
        base.FrontLeft();
        smear.facingDirection = 1;
    }

    public override void BackLeft()
    {
        base.BackLeft();
        smear.facingDirection = 2;
    }

    public override void BackRight()
    {
        base.BackRight();
        smear.facingDirection = 3;
    }
}
