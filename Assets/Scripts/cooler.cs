using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cooler : MonoBehaviour
{
    ParticleSystem bubbles;
    float timer = 0.3f;

    private void Start()
    {
        bubbles = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            bubbles.Play();
            timer = Random.Range(2f, 7f);
        }
    }
}
