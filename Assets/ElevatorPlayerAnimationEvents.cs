using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlayerAnimationEvents : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EndOfIdle()
    {
        float randFloat = Random.Range(0.0f, 1.0f);

        if(randFloat < 0.1f )
        {
            animator.Play("HandsOnHips");
        }
        else if(randFloat < 0.2f)
        {
            animator.Play("CheckWatch");
        }
    }

    public void EndOfHandsOnHips()
    {
        float randFloat = Random.Range(0.0f, 1.0f);

        if (randFloat < 0.3f)
        {
            animator.Play("HandsOnHips3");
        }
    }
}
