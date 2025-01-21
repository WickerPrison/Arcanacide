using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuFatmanAminations : MonoBehaviour
{
    Animator animator;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        timer = Random.Range(0, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            PlayAnimation();
            timer = Random.Range(15f, 30f);
        }
    }

    void PlayAnimation()
    {
        animator.Play("FatLaugh");


        //float randVal = Random.Range(0f, 1f);
        //if (randVal < 0.2f)
        //{
        //    animator.Play("ButtScratch");
        //}
        //else
        //{
        //    animator.Play("SwordmanNod");
        //}
    }
}
