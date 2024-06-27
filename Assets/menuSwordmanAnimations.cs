using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuSwordmanAnimations : MonoBehaviour
{
    Animator animator;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        timer = Random.Range(0, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            PlayAnimation();
            timer = Random.Range(5f, 20f);
        }
    }

    void PlayAnimation()
    {

        float randVal = Random.Range(0f, 1f);
        if(randVal < 0.2f)
        {
            animator.Play("ButtScratch");
        }
        else
        {
            animator.Play("SwordmanNod");
        }
    }
}
