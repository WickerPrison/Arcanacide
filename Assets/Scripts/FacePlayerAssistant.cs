using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FacePlayerAssistant : FacePlayer
{
    [SerializeField] Transform frontBody;
    [SerializeField] Transform backBody;
    float frontBodyScaleX;
    float backBodyScaleX;

    public override void Start()
    {
        base.Start();
        frontBodyScaleX = frontBody.localScale.x;
        backBodyScaleX = backBody.localScale.x;
    }

    public override void Update()
    {
        AttackPoint();

        FaceAttackPoint();
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

    public override void FrontRight()
    {
        faceDirectionID = 0;
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontBody.localScale = new Vector3(frontBodyScaleX, frontBody.localScale.y, frontBody.localScale.z);
    }

    public override void FrontLeft()
    {
        faceDirectionID = 1;
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = new Vector3(-frontAnimatorPosition.x, frontAnimatorPosition.y, frontAnimatorPosition.z);
        frontBody.localScale = new Vector3(-frontBodyScaleX, frontBody.localScale.y, frontBody.localScale.z);
    }

    public override void BackRight()
    {
        faceDirectionID = 3;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        backBody.localScale = new Vector3(backBodyScaleX, backBody.localScale.y, backBody.localScale.z);
    }

    public override void BackLeft()
    {
        faceDirectionID = 2;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = new Vector3(-backAnimatorPosition.x, backAnimatorPosition.y, backAnimatorPosition.z);
        backBody.localScale = new Vector3(-backBodyScaleX, backBody.localScale.y, backBody.localScale.z);
    }
}
