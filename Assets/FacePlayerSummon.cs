using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerSummon : FacePlayer
{
    public override void Update()
    {
        //override with nothing
    }

    public override void FaceAttackPoint()
    {
        if (attackPoint.position.z < transform.position.z)
        {
            if (attackPoint.position.x > transform.position.x)
            {
                FrontRight();
            }
            else
            {
                FrontLeft();
            }
        }
        else
        {
            if (attackPoint.position.x > transform.position.x)
            {
                BackRight();
            }
            else
            {
                BackLeft();
            }
        }
    }
}
