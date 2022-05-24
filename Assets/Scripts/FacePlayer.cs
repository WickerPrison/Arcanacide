using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public Transform attackPoint;
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    EnemyController enemyController;
    public PlayerController playerController;
    Vector3 frontAnimatorPosition;
    Vector3 backAnimatorPosition;
    public Vector3 away = new Vector3(100, 100, 100);
    float scaleX;

    public virtual void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyController = GetComponent<EnemyController>();
        scaleX = frontAnimator.transform.localScale.x;
        frontAnimatorPosition = frontAnimator.transform.localPosition;
        backAnimatorPosition = backAnimator.transform.localPosition;
    }

    private void Update()
    {
        if (enemyController.navAgent.enabled && !enemyController.directionLock)
        {
            AttackPoint();

            FacePlayerSprite();
        }
    }


    public virtual void AttackPoint()
    {
        Vector3 direction = playerController.transform.position - transform.position;
        direction = new Vector3(direction.x, 0, direction.z);
        attackPoint.position = transform.position + direction.normalized;
        attackPoint.transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    public void FacePlayerSprite()
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

    void FrontRight()
    {
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(scaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
    }

    void FrontLeft()
    {
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = new Vector3(-frontAnimatorPosition.x, frontAnimatorPosition.y, frontAnimatorPosition.z);
        frontAnimator.transform.localScale = new Vector3(-scaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
    }

    void BackRight()
    {
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        backAnimator.transform.localScale = new Vector3(scaleX, backAnimator.transform.localScale.y, backAnimator.transform.localScale.z);
    }

    void BackLeft()
    {
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = new Vector3(-backAnimatorPosition.x, backAnimatorPosition.y, backAnimatorPosition.z);
        backAnimator.transform.localScale = new Vector3(-scaleX, backAnimator.transform.localScale.y, backAnimator.transform.localScale.z);
    }
}
