using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform attackPoint;
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    EnemyController enemyController;
    [System.NonSerialized] public Transform player;
    Vector3 frontAnimatorPosition;
    Vector3 backAnimatorPosition;
    public Vector3 away = new Vector3(100, 100, 100);
    float frontScaleX;
    float backScaleX;
    public Vector3 faceDirection;
    Vector3 faceDestination;
    bool facePlayer = true;
    [System.NonSerialized] public int faceDirectionID;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyController = GetComponent<EnemyController>();
        frontScaleX = frontAnimator.transform.localScale.x;
        backScaleX = backAnimator.transform.localScale.x;
        frontAnimatorPosition = frontAnimator.transform.localPosition;
        backAnimatorPosition = backAnimator.transform.localPosition;
    }

    private void Update()
    {
        if (enemyController.navAgent.enabled && !enemyController.directionLock)
        {
            AttackPoint();

            FaceAttackPoint();
        }
    }

    public void SetDestination(Vector3 destination)
    {
        facePlayer = false;
        faceDestination = destination;
    }

    public void ResetDestination()
    {
        facePlayer = true;
    }

    public void ManualFace()
    {
        AttackPoint();
        FaceAttackPoint();
    }

    public virtual void AttackPoint()
    {
        if (facePlayer)
        {
            faceDirection = player.position - transform.position;
        }
        else
        {
            faceDirection = faceDestination - transform.position;
        }

        faceDirection = new Vector3(faceDirection.x, 0, faceDirection.z).normalized;
        attackPoint.position = transform.position + faceDirection;
        if(faceDirection.magnitude > 0)
        {
            attackPoint.transform.localRotation = Quaternion.LookRotation(faceDirection);
        }
    }

    public void FaceAttackPoint()
    {
        if (attackPoint.position.z < transform.position.z)
        {
            enemyController.facingFront = true;
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
            enemyController.facingFront = false;
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

    public void FaceDirection(int directionID)
    {
        switch (directionID)
        {
            case 0:
                FrontRight();
                break;
            case 1:
                FrontLeft();
                break;
            case 2:
                BackLeft();
                break;
            case 3:
                BackRight();
                break;
        }
    }

    public virtual void FrontRight()
    {
        faceDirectionID = 0;
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(frontScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
    }

    public virtual void FrontLeft()
    {
        faceDirectionID = 1;
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = new Vector3(-frontAnimatorPosition.x, frontAnimatorPosition.y, frontAnimatorPosition.z);
        frontAnimator.transform.localScale = new Vector3(-frontScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
    }

    public virtual void BackRight()
    {
        faceDirectionID = 3;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        backAnimator.transform.localScale = new Vector3(backScaleX, backAnimator.transform.localScale.y, backAnimator.transform.localScale.z);
    }

    public virtual void BackLeft()
    {
        faceDirectionID = 2;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = new Vector3(-backAnimatorPosition.x, backAnimatorPosition.y, backAnimatorPosition.z);
        backAnimator.transform.localScale = new Vector3(-backScaleX, backAnimator.transform.localScale.y, backAnimator.transform.localScale.z);
    }
}
