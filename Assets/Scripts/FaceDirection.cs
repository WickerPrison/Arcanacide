using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirections
{
    FRONT_LEFT, FRONT_RIGHT, BACK_LEFT, BACK_RIGHT
}

public class FaceDirection : MonoBehaviour
{
    [SerializeField] Transform frontAnimator;
    [SerializeField] Transform backAnimator;
    Vector3 away = new Vector3(100, 100, 100);
    Vector3 frontAnimatorPosition;
    Vector3 backAnimatorPosition;
    float frontScaleX;
    float backScaleX;
    Transform player;
    [System.NonSerialized] public bool facingFront;

    private void Awake()
    {
        frontAnimatorPosition = frontAnimator.transform.localPosition;
        frontScaleX = frontAnimator.transform.localScale.x;
        if(backAnimator != null)
        {
            backAnimatorPosition = backAnimator.transform.localPosition;
            backScaleX = backAnimator.transform.localScale.x;
        }
    }

    public void FacePlayer()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        FaceTowards(player.position);
    }


    public void FaceTowards(Vector3 target)
    {
        bool faceFront = target.z < transform.position.z;
        bool faceRight = target.x >= transform.position.x;

        if (faceFront)
        {
            if (faceRight) DirectionalFace(FacingDirections.FRONT_RIGHT);
            else DirectionalFace(FacingDirections.FRONT_LEFT);
        }
        else
        {
            if (faceRight) DirectionalFace(FacingDirections.BACK_RIGHT);
            else DirectionalFace(FacingDirections.BACK_LEFT);
        }
    }

    public void DirectionalFace(FacingDirections direction)
    {
        switch (direction)
        {
            case FacingDirections.FRONT_LEFT:
                facingFront = true;
                backAnimator.transform.localPosition = away;
                frontAnimator.transform.localPosition = frontAnimatorPosition;
                frontAnimator.transform.localScale = new Vector3(
                    -frontScaleX,
                    frontAnimator.transform.localScale.y,
                    frontAnimator.transform.localScale.z);
                break;
            case FacingDirections.FRONT_RIGHT:
                facingFront = true;
                backAnimator.transform.localPosition = away;
                frontAnimator.transform.localPosition = frontAnimatorPosition;
                frontAnimator.transform.localScale = new Vector3(
                    frontScaleX,
                    frontAnimator.transform.localScale.y,
                    frontAnimator.transform.localScale.z);
                break;
            case FacingDirections.BACK_LEFT:
                if (backAnimator == null) goto case FacingDirections.FRONT_LEFT;
                facingFront = false;
                backAnimator.transform.localPosition = backAnimatorPosition;
                frontAnimator.transform.localPosition = away;
                backAnimator.transform.localScale = new Vector3(
                    -backScaleX,
                    frontAnimator.transform.localScale.y,
                    frontAnimator.transform.localScale.z);
                break;
            case FacingDirections.BACK_RIGHT:
                if (backAnimator == null) goto case FacingDirections.FRONT_RIGHT;
                facingFront = false;
                backAnimator.transform.localPosition = backAnimatorPosition;
                frontAnimator.transform.localPosition = away;
                backAnimator.transform.localScale = new Vector3(
                    backScaleX,
                    frontAnimator.transform.localScale.y,
                    frontAnimator.transform.localScale.z);
                break;
        }
    }
}
