using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    ParticleSystem particles;
    Collider capsuleCollider;
    float delay = 0.5f;
    float duration = 1;
    int damage = 20;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        capsuleCollider = GetComponent<Collider>();
        capsuleCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerScript playerScript;
            playerScript = other.gameObject.GetComponent<PlayerScript>();
            playerScript.LoseHealth(damage);
        }
    }

    private void Update()
    {
        if(delay > 0)
        {
            indicator.transform.localScale *= 1 + Time.deltaTime * 2;
        }

        duration -= Time.deltaTime;
        delay -= Time.deltaTime;

        if(delay <= 0)
        {
            if (!particles.isPlaying)
            {
                particles.Play();
            }
            capsuleCollider.enabled = true;
        }
        if(duration <= 0)
        {
            Destroy(gameObject);
        }
    }
}
